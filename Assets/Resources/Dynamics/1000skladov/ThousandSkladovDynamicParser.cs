using HtmlAgilityPack;
using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityParser;
using Zenject;

namespace InGame.Dynamics
{
    public class ThousandSkladovDynamicParser : DynamicParser
    {
        private IInputField url;
        private IPaging paging;
        private ISelectTable table;

        private IWebClient client;
        private HtmlDocument doc = new();

        [Inject]
        private void Construct(IInputField url, IPaging paging, ISelectTable table, IWebClient client)
        {
            this.url = url;
            this.paging = paging;
            this.table = table;
            this.client = client;

            url.Setup(new()
            {
                labelText = "������ �� 1000skladov � ���������",
                placeholderText = "������"
            });
            paging.Setup(new IPaging.PageModel() { pagePattern = "page-{0}"});
            table.Setup(new());

            BakeElements();
        }

        protected override void OnStart()
        {
            client.Download(url.Text, doc);

            IParseResult bigResult = new ParseResult<StorageLot>();
            paging.RequestPagesCount = 1000;

            for (int i = 0; i < paging.Count; i++)
            {
                int pageIndex = paging.Start + i;

                status.Status = "�������� ��������";
                if (paging.End == paging.RequestPagesCount) status.Progress = i.ToString();
                else status.Progress = i + "/" + paging.Count;


                string currentUrl = paging.GetPagedUrl(url.Text, pageIndex);

                client.Download(currentUrl, doc);

                if (HasItemsFound())
                {
                    ParseResult<StorageLot> result = ParsePage();
                    bigResult.AddRange(result.lots);
                }
                else
                {
                    Debug.Log("Items not found");
                    break;
                }
            }

            table.SaveResult(bigResult);
        }

        protected override void OnStop()
        {
            
        }
        private bool HasItemsFound()
        {
            HtmlNode container = doc.DocumentNode.SelectSingleNode(".//div[@class='row ct-js-search-results ct-showProducts--list']");
            return container.InnerText.ToLower().Contains("������ ������� ������ �� �������") == false;
        }
        private ParseResult<StorageLot> ParsePage()
        {
            var nodes = GetNodes();
            Debug.Log("Nodes urls: " + string.Join("\n", nodes.Select(n => n.GetAttributeValue("href", "-"))));

            ParseResult<StorageLot> result = new ParseResult<StorageLot>();
            result.AddRange(ParseLots(nodes));

            return result;
        }
        private IEnumerable<HtmlNode> GetNodes()
        {
            HtmlNode container = doc.DocumentNode.SelectSingleNode(".//div[@class='row ct-js-search-results ct-showProducts--list']");
            foreach (HtmlNode node in container.ChildNodes)
            {
                if (node.ChildNodes.Count == 0) continue;

                yield return node.SelectSingleNode(".//a");
            }
        }
        private IEnumerable<StorageLot> ParseLots(IEnumerable<HtmlNode> nodes)
        {
            foreach (HtmlNode node in nodes)
            {
                yield return ParseLotOrThrowException(node);
            }
        }
        private StorageLot ParseLotOrThrowException(HtmlNode node)
        {
            string url = "https://1000skladov.ru" + node.GetAttributeValue("href", "");
            StorageLot lot = new StorageLot(url);

            client.Download(url, doc);

            HtmlNode title = doc.DocumentNode.SelectSingleNode(".//h1[@class='text-uppercase page-title']");
            lot.name = title.InnerText;


            var infoContainer = doc.DocumentNode.SelectSingleNode(".//div[@class='ct-u-displayTableVertical ct-productDetails ct-u-marginBottom40']");
            foreach (HtmlNode infoNode in infoContainer.ChildNodes)
            {
                if (infoNode.ChildNodes.Count == 0) continue;
                var nodes = infoNode.SelectNodes(".//span");
                if (nodes.Count != 2) continue;

                GetField(lot, nodes[0].InnerText) = nodes[1].InnerText;
            }


            var additionalInfoContainer = doc.DocumentNode.SelectSingleNode(".//ul[@class='list-unstyled']");

            lot.additionalInfo = string.Join(";",
                additionalInfoContainer.ChildNodes.Where(n => string.IsNullOrWhiteSpace(n.InnerText) == false)
                .Select((n) => n.InnerText.Trim()));


            return lot;
        }

        private ref string GetField(StorageLot lot, string type)
        {
            type = type.ToLower();

            switch (type)
            {
                case "��� ������": return ref lot.contractType;
                case "������������": return ref lot.address;
                case "����": return ref lot.costPerMeter;
                case "�������": return ref lot.square;
                case "������� ���� � ����": return ref lot.additionalInfo;
                default: throw new System.Exception($"Failed to define field with type '{type}' in lot '{nameof(StorageLot)}'");
            }
        }
    }

    public class StorageLot : Lot
    {
        [ExcelString("��������")]
        public string name;
        [ExcelString("��� ������")]
        public string contractType;
        [ExcelString("�����")]
        public string storageClass;
        [ExcelString("�����")]
        public string metro;
        [ExcelString("�����")]
        public string address;
        [ExcelString("���� �� ����")]
        public string costPerMeter;
        [ExcelString("����")]
        public string costWhole;
        [ExcelString("�������")]
        public string square;
        [ExcelString("���. ����������")]
        public string additionalInfo;

        public StorageLot() : base() { }
        public StorageLot(string url) : base(url) { }
    }
}