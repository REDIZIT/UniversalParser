using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class ThousandSkladovParser : Parser<StorageLot>
    {
        protected override string UrlPageArgument => "page";

        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            HtmlNode container = doc.DocumentNode.SelectSingleNode(".//div[@class='row ct-js-search-results ct-showProducts--list']");
            foreach (HtmlNode node in container.ChildNodes)
            {
                if (node.ChildNodes.Count == 0) continue;

                yield return node.SelectSingleNode(".//a");
            }
        }

        protected override StorageLot ParseLotOrThrowException(HtmlNode node)
        {
            string url = "https://1000skladov.ru" + node.GetAttributeValue("href", "");
            StorageLot lot = new StorageLot(url);

            HtmlNode doc = DownloadHtml(url).DocumentNode;


            HtmlNode title = doc.SelectSingleNode(".//h1[@class='text-uppercase page-title']");
            lot.name = title.InnerText;


            var infoContainer = doc.SelectSingleNode(".//div[@class='ct-u-displayTableVertical ct-productDetails ct-u-marginBottom40']");
            foreach (HtmlNode infoNode in infoContainer.ChildNodes)
            {
                if (infoNode.ChildNodes.Count == 0) continue;
                var nodes = infoNode.SelectNodes(".//span");
                if (nodes.Count != 2) continue;

                GetField(lot, nodes[0].InnerText) = nodes[1].InnerText;
            }


            var additionalInfoContainer = doc.SelectSingleNode(".//ul[@class='list-unstyled']");

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