using HtmlAgilityPack;
using System.Collections.Generic;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class SpbAfyParser : BrowserParser<MirkvartirLot>
    {
        protected override string UrlPageArgument => "page";

        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            HtmlNode listNode = doc.DocumentNode.SelectSingleNode(".//div[@class='object-list']").ChildNodes[1];

            foreach (HtmlNode node in listNode.ChildNodes)
            {
                if (node.GetAttributeValue("data-role", null) == "pbbitem_in_list")
                {
                    yield return node;
                }
            }
        }

        protected override MirkvartirLot ParseLotOrThrowException(HtmlNode node)
        {
            MirkvartirLot lot = new MirkvartirLot();

            //
            HtmlNode urlButton = node.SelectSingleNode(".//a[@class='object-item-info-more-link']");
            lot.url = urlButton.GetAttributeValue("href", null);


            HtmlNode titleNode = node.SelectSingleNode(".//a[@class='object-item-head-title']");
            if (titleNode == null)
            {
                titleNode = node.SelectSingleNode(".//a[@class='pbbitem-premium-head-title']");
            }
            lot.rooms = titleNode.InnerText;


            HtmlNode address = node.SelectSingleNode(".//div[@class='object-item-info-geo-wrap']");
            if (address == null)
            {
                address = node.SelectSingleNode(".//div[@class='pbbitem-premium-info-location']");
            }
            lot.address = address.InnerText;

          

            HtmlNode characteristicsNode = node.SelectSingleNode(".//ul[@class='object-item-info-numbers-characteristics']");
            if (characteristicsNode == null)
            {
                characteristicsNode = node.SelectSingleNode(".//ul[@class='pbbitem-premium-info-prop-list']");
            }

            foreach (HtmlNode child in characteristicsNode.ChildNodes)
            {
                if (child.InnerText.Contains("площадь"))
                {
                    lot.area = child.InnerText.Replace("площадь", "").Trim();
                }
                else if (child.InnerText.Contains("этаж"))
                {
                    lot.floor = child.InnerText;
                }
            }



            HtmlNode priceNode = node.SelectSingleNode(".//span[@class='object-item-second-price-sum']");
            if (priceNode == null)
            {
                priceNode = node.SelectSingleNode(".//span[@class='pbbitem-premium-info-price-wrap']");
            }

            lot.price = priceNode.InnerText;

            return lot;
        }
    }
}