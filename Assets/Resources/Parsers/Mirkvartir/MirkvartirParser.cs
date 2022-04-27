using HtmlAgilityPack;
using InGame.Recognition;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.Parse
{
    public class MirkvartirParser : BrowserParser<MirkvartirLot>
    {
        protected override string UrlPageArgument => "p";

        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode(".//div[@class='b-flats-list']").ChildNodes.Where(n => n.HasClass("b-flat"));
        }
        protected override MirkvartirLot ParseLotOrThrowException(HtmlNode node)
        {
            HtmlNode urlNode = node.SelectSingleNode(".//div[@class='img']/a");

            MirkvartirLot lot = new MirkvartirLot(urlNode.GetAttributeValue("href", "<url not found>"));


            #region Rooms, area, floor (title)

            HtmlNode title = node.SelectSingleNode(".//a[@class='offer-title']/span");

            Recognizer.Result titleExtractResult = ExtractInfoFromTitle(title.InnerText);
            lot.rooms = titleExtractResult.name;
            lot.area = titleExtractResult.area;
            lot.floor = titleExtractResult.storeys;

            #endregion

            #region Price

            HtmlNode priceNode = node.SelectSingleNode(".//div[2]/div[2]/div[2]/div[1]/span");

            lot.price = priceNode.InnerText;

            #endregion

            #region Address

            HtmlNode addressNode = node.SelectSingleNode(".//div[@class='address']");
            lot.address = addressNode.InnerText;

            #endregion

            HtmlNode placeNode = node.SelectSingleNode(".//div[@class='address']");
            lot.metroOrDistrict = placeNode.InnerText;

            #region Metro



            #endregion

            return lot;
        }


        private Recognizer.Result ExtractInfoFromTitle(string titleText)
        {
            string[] split = titleText.Split(',');

            if (split.Length >= 3)
            {
                return new Recognizer.Result
                {
                    name = split[0].Trim(),
                    area = split[1].Trim(),
                    storeys = split[2].Trim()
                };
            }
            else if (split.Length == 2)
            {
                return new Recognizer.Result
                {
                    area = split[0].Trim(),
                    storeys = split[1].Trim()
                };
            }
            else if (split.Length == 1)
            {
                return new Recognizer.Result
                {
                    area = split[0].Trim()
                };
            }
            else
            {
                throw new System.Exception($"Can't extract info from title '{titleText}'");
            }
        }
    }
}