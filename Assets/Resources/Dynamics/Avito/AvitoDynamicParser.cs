using HtmlAgilityPack;
using InGame.Parse;
using InGame.Recognition;
using RestSharp.Contrib;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Zenject;

namespace InGame.Dynamics
{
    public class AvitoDynamicParser : DynamicParser
    {
        private IInputField url;
        private IPaging paging;
        private ISelectTable table;

        private IBrowser browser;

        private HtmlDocument doc = new();

        [Inject]
        private void Construct(IInputField url, IPaging paging, ISelectTable table, IBrowser browser)
        {
            this.url = url;
            this.paging = paging;
            this.table = table;
            this.browser = browser;

            url.Setup(new()
            {
                labelText = "������ �� ����� � ���������",
                placeholderText = "������"
            });
            paging.Setup(new IPaging.ArgumentModel());
            table.Setup(new());

            BakeElements();
        }
        protected override void OnStart()
        {
            browser.Open();

            paging.RequestPagesCount = GetPagesCount();

            IParseResult bigResult = new ParseResult<AvitoLot>();

            for (int i = 0; i < paging.Count; i++)
            {
                int pageIndex = paging.Start + i;

                status.Status = "�������� ��������";
                status.Progress = i + "/" + paging.Count;

                ParseResult<AvitoLot> result = ParsePage(paging.GetPagedUrl(url.Text, pageIndex));
                bigResult.AddRange(result.lots);
            }

            table.SaveResult(bigResult);
        }

        protected override void OnStop()
        {
            browser.Close();
        }

        private int GetPagesCount()
        {
            // pagination-button
            browser.GoToUrl(paging.GetPagedUrl(url.Text, paging.Start));
            browser.GetDocument(doc);
            var pageGroup = doc.DocumentNode.SelectSingleNode(".//div[@data-marker='pagination-button']");
            var lastPage = pageGroup.ChildNodes[^2];

            return int.Parse(lastPage.InnerText);
        }
        private ParseResult<AvitoLot> ParsePage(string url)
        {
            browser.GoToUrl(url);
            browser.GetDocument(doc);

            var nodes = GetNodesToParse(doc);

            ParseResult<AvitoLot> result = new ParseResult<AvitoLot>();
            result.AddRange(ParseLots(nodes));

            return result;
        }
        private IEnumerable<AvitoLot> ParseLots(IEnumerable<HtmlNode> nodes)
        {
            foreach (HtmlNode node in nodes)
            {
                yield return ParseLotOrThrowException(node);
            }
        }
        protected IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            HtmlNode content = doc.DocumentNode.SelectSingleNode(".//div[@data-marker='catalog-serp']");
            if (content == null)
            {
                throw new System.NullReferenceException("Content is null");
            }

            IEnumerable<HtmlNode> nodes = content.ChildNodes.Where(n => n.Attributes.Any(a => a.Name == "data-marker" && a.Value == "item"));

            return nodes;
        }
        protected AvitoLot ParseLotOrThrowException(HtmlNode node)
        {
            HtmlNode titleNode = node.SelectSingleNode(".//a[@itemprop='url'][@data-marker='item-title']");
            string url = "https://avito.ru" + titleNode.GetAttributeValue("href", null);

            AvitoLot lot = new AvitoLot(url);

            #region Title (name, area, storyes)

            string text = HttpUtility.HtmlDecode(titleNode.ChildNodes[0].InnerText);

            // Get area from recognizer and remove this from name text
            if (Recognizer.TryRecognize(text, out Recognizer.Result result))
            {
                lot.name = result.name;
                lot.area = result.area;
                lot.storeys = result.storeys;
            }
            else
            {
                // For flat
                string[] split = SplitTitle(text).ToArray();
                if (split.Length >= 4 || (split.Length >= 2 && RecognizerArea.IsAreaString(split[1]) == false))
                {
                    lot.name = text;
                }
                else
                {
                    if (split.Length >= 1) lot.name = split[0];
                    if (split.Length >= 2) lot.area = split[1];
                    if (split.Length >= 3) lot.storeys = split[2];
                }
            }



            #endregion


            #region Price

            HtmlNode priceNode = node.SelectSingleNode(".//span[@data-marker='item-price']");
            HtmlNode priceSpan = priceNode.SelectSingleNode(".//span");
            lot.price = priceSpan.InnerText.Replace("&nbsp;", " ");

            #endregion

            #region OnlineView

            lot.hasOnlineView = node.InnerHtml.Contains("������-�����");

            #endregion

            #region Address and metro

            var addressNode = node.SelectSingleNode(".//div[@data-marker='item-address']").FirstChild;

            var metroNode = addressNode.SelectSingleNode(".//div[@class='geo-georeferences-3or5Q text-text-1PdBw text-size-s-1PUdo']");
            lot.address = addressNode.InnerText;

            if (metroNode != null)
            {
                lot.metro = metroNode.InnerText;
                lot.address = lot.address.Replace(lot.metro, "");
            }


            #endregion

            #region Agency

            var agencyNode = node.SelectSingleNode(".//a[@rel='noopener'][@data-marker='item-link']");
            if (agencyNode != null)
            {
                lot.agency = agencyNode.InnerText;
            }

            #endregion

            return lot;
        }
        private IEnumerable<string> SplitTitle(string titleText)
        {
            MatchEvaluator evaluator = new MatchEvaluator(MatchEvalute);
            titleText = Regex.Replace(titleText, @"\d,\d", evaluator);

            foreach (string str in titleText.Split(','))
            {
                yield return str.Replace("/*.*/", ",").Trim();
            }
        }

        private string MatchEvalute(Match match)
        {
            string text = match.Value;

            return text.Replace(",", "/*.*/");
        }
    }
}