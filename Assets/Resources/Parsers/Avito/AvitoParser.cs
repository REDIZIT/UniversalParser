using HtmlAgilityPack;
using InGame.Recognition;
using RestSharp.Contrib;
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

            // Get area from recognizer and remove this from name text
            string recognizedArea = RecognizerArea.TryExtractAreaString(text);
            if (string.IsNullOrEmpty(recognizedArea) == false)
            {
                string name = text.Replace(recognizedArea, "");
                lot.name = name;
                lot.area = recognizedArea;
            }
            else
            {
                // For flat
                string[] split = SplitTitle(text).ToArray();
                if (split.Length > 1) lot.name = split[0];
                if (split.Length > 2) lot.area = split[1];
                if (split.Length > 3) lot.storeys = split[2];
            }



            #endregion


            #region Price

            HtmlNode priceNode = node.SelectSingleNode(".//span[@data-marker='item-price']");
            HtmlNode priceSpan = priceNode.SelectSingleNode(".//span");
            lot.price = priceSpan.InnerText;

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
                lot.agency = HttpUtility.HtmlDecode(agencyNode.InnerText);
                //lot.agency = agencyNode.InnerText;
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