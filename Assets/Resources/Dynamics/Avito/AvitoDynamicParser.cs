using HtmlAgilityPack;
using InGame.Parse;
using InGame.Recognition;
using RestSharp.Contrib;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Zenject;

namespace InGame.Dynamics
{
    public class AvitoDynamicParser : DynamicParser
    {
        private IInputField url, timeout;
        private IPaging paging;
        private ISelectTable table;

        private IBrowser browser;

        private HtmlDocument doc = new();

        [Inject]
        private void Construct(IInputField url, IPaging paging, ISelectTable table, IBrowser browser, IInputField timeout)
        {
            this.url = url;
            this.paging = paging;
            this.table = table;
            this.browser = browser;
            this.timeout = timeout;

            url.Setup(new()
            {
                labelText = "Ссылка на авито с фильтрами",
                placeholderText = "Ссылка"
            });
            paging.Setup(new IPaging.ArgumentModel());
            table.Setup(new());
            timeout.Setup(new()
            {
                labelText = "Время (сек) между запросами",
                placeholderText = "Время в секундах",
                isNumberField = true,
                defaultText = "5"
            });

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

                status.Status = "Скачиваю страницу";
                status.Progress = i + "/" + paging.Count;

                ParseResult<AvitoLot> result = ParsePage(paging.GetPagedUrl(url.Text, pageIndex));
                bigResult.AddRange(result.lots);

                if (int.TryParse(timeout.Text, out int seconds) && seconds > 0)
                {
                    status.Status = "Ожидание перед следующим запросом (" + seconds + ")";
                    Thread.Sleep(1000 * int.Parse(timeout.Text));
                }
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

            var list = doc.DocumentNode.SelectSingleNode(".//ul[@data-marker='pagination-button']");

            var lastPage = list.ChildNodes[list.ChildNodes.Count - 2];

            UnityEngine.Debug.Log("Last page = " + lastPage.InnerText);

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

            HtmlNode priceNode = node.SelectSingleNode(".//p[@data-marker='item-price']");
            string[] priceSplit = priceNode.InnerText.Split("&nbsp;");
            lot.price = string.Join("", priceSplit.Take(priceSplit.Length - 1));

            #endregion

            #region OnlineView

            lot.hasOnlineView = node.InnerHtml.Contains("Онлайн-показ");

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