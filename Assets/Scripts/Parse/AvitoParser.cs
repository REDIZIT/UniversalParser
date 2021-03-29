using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityParser;

namespace InGame.Parse
{
    public class AvitoParser : Parser<AvitoLot>
    {
        protected override AvitoLot ParseLotOrThrowException(HtmlNode node)
        {
            AvitoLot lot = new AvitoLot();

            #region Title (name, area, storyes)

            HtmlNode titleNode = node.SelectSingleNode(".//a[@itemprop='url'][@data-marker='item-title']");

            lot.url = "https://avito.ru" + titleNode.GetAttributeValue("href", null);

            string text = titleNode.ChildNodes[0].InnerText;
            string[] split = SplitTitle(text).ToArray();

            lot.name = split[0];
            lot.area = split[1];
            lot.storeys = split[2];

            #endregion


            #region Price

            HtmlNode priceNode = node.SelectSingleNode(".//span[@data-marker='item-price']");
            HtmlNode priceSpan = priceNode.SelectSingleNode(".//span");
            lot.price = priceSpan.InnerText.Replace(" ₽", "");

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

            //// If only address
            //if (addressNode.ChildNodes.Count == 1)
            //{
            //    //lot.address = addressNode.SelectSingleNode(".//span").InnerText;
            //    lot.address = addressNode.InnerText;
            //}
            //else
            //{
            //    var addressSpan = addressNode.SelectSingleNode(".//span");
            //    lot.address = addressSpan.SelectSingleNode(".//span").InnerText;

            //    var metroDiv = addressNode.SelectSingleNode(".//div");
            //    var metroSpans = metroDiv.SelectNodes(".//span");
            //    lot.metro = string.Concat(metroSpans.Select(n => n.InnerText));
            //}


            #endregion

            #region Agency

            var agencyNode = node.SelectSingleNode(".//a[@rel='noopener'][@data-marker='item-link']");
            if (agencyNode != null)
            {
                //lot.agency = HttpUtility.HtmlDecode(agencyNode.InnerText);
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