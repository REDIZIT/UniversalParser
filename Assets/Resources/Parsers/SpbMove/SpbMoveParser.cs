using HtmlAgilityPack;
using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityParser;

namespace Assets.Resources.Parsers.SpbMove
{
    public class SpbMoveParser : BrowserParser<MirkvartirLot>
    {
        protected override string UrlPageArgument => "page";

        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            HtmlNode container = doc.DocumentNode.SelectSingleNode(".//div[@id='vue-app-items']");
            HtmlNode firstChild = container.ChildNodes[0];

            foreach (HtmlNode item in firstChild.ChildNodes[10].ChildNodes)
            {
                if (item.HasClass("search-item"))
                {
                    yield return item;
                }
            }
        }

        protected override MirkvartirLot ParseLotOrThrowException(HtmlNode node)
        {
            MirkvartirLot lot = new MirkvartirLot();

            HtmlNode aNode = node.SelectSingleNode(".//a");
            lot.url = aNode.GetAttributeValue("href", "");

            string title = aNode.GetAttributeValue("title", "");
            lot.address = GetAddressFromTitle(title);

            lot.price = node.SelectSingleNode(".//div[@class='search-item__price-values']").InnerText;



            HtmlNode properties = node.SelectSingleNode(".//ul[@class='search-item__properties']");
            foreach (HtmlNode item in properties.ChildNodes)
            {
                if (item.HasClass("search-item__metro"))
                {
                    lot.metroOrDistrict = item.InnerText;
                }
                else if (item.InnerText.ToLower().Contains("площадь"))
                {
                    lot.area = item.InnerText.Replace("Площадь:", "");
                }
                else if (item.InnerText.ToLower().Contains("этаж"))
                {
                    lot.floor = item.InnerText.Replace("Этаж:", "");
                }
            }


            return lot;
        }

        private string GetAddressFromTitle(string title)
        {
            IEnumerable<string> splitted = title.Split(',');

            string result = string.Join(" ", splitted.Skip(2));

            return result;
        }
    }
}