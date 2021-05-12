using HtmlAgilityPack;
using InGame.Recognition;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.Parse
{
    public class MirkvartirParser : Parser<MirkvatrirLot>
    {
        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            return doc.DocumentNode.SelectSingleNode(".//div[@class='b-flats-list']").ChildNodes.Where(n => n.HasClass("b-flat"));
        }
        protected override MirkvatrirLot ParseLotOrThrowException(HtmlNode node)
        {
            HtmlNode urlNode = node.SelectSingleNode(".//div[@class='img']/a");

            MirkvatrirLot lot = new MirkvatrirLot(urlNode.GetAttributeValue("href", "<url not found>"));


            #region Rooms, area, floor (title)

            HtmlNode title = node.SelectSingleNode(".//a[@class='offer-title']/span");

            Recognizer.Result titleExtractResult = ExtractInfoFromTitle(title.InnerText);
            lot.rooms = titleExtractResult.name;
            lot.area = titleExtractResult.area;
            lot.floor = titleExtractResult.storeys;

            #endregion

            #region Price

            HtmlNode priceNode = node.SelectSingleNode(".//div[2]/div[2]/div[2]/div[1]/span");
            //HtmlNode priceNode2 = node.SelectSingleNode(".//div[1]/div[1]/div[1]/div[0]");
            //HtmlNode priceNode3 = node.SelectSingleNode(".//div[1]/div[1]/div[1]");
            //HtmlNode priceNode4 = node.SelectSingleNode(".//div[1]/div[1]");
            //HtmlNode priceNode5 = node.SelectSingleNode(".//div[1]");

            lot.price = priceNode.InnerText;

            #endregion

            return lot;
        }


        private Recognizer.Result ExtractInfoFromTitle(string titleText)
        {
            string[] split = titleText.Split(',');

            Recognizer.Result result = new Recognizer.Result
            {
                name = split[0].Trim(),
                area = split[1].Trim(),
                storeys = split[2].Trim()
            };

            return result;
        }
    }
}