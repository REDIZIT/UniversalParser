using HtmlAgilityPack;
using InGame.Dynamics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class Spb4rentParser : BrowserParser<StorageLot>
    {
        protected override string UrlPageArgument => "PAGEN_2";

        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            foreach (HtmlNode node in doc.DocumentNode.SelectNodes(".//div[@class='mb-3 item card']"))
            {
                yield return node.SelectSingleNode(".//a");
            }
        }

        protected override StorageLot ParseLotOrThrowException(HtmlNode node)
        {
            string url = "https://spb4rent.com" + node.GetAttributeValue("href", "");
            StorageLot lot = new StorageLot(url);

            HtmlNode doc = DownloadHtml(url).DocumentNode;

            lot.name = doc.SelectSingleNode(".//h1[@class='col-md-8 col-sm-12']").InnerText;

            foreach (HtmlNode propNode in doc.SelectSingleNode(".//div[@class='props']").ChildNodes)
            {
                if (propNode.ChildNodes.Count == 0) continue;
                GetLotField(lot, propNode.GetClasses().First()) = propNode.InnerText;
            }

            lot.additionalInfo = string.Join(";",
                doc.SelectSingleNode(".//div[@class='other-props']")
                .SelectNodes(".//li")
                .Select(n => n.InnerText));

            return lot;
        }

        private ref string GetLotField(StorageLot lot, string propName)
        {
            switch (propName)
            {
                case "class": return ref lot.storageClass;
                case "metro": return ref lot.metro;
                case "location": return ref lot.address;
                case "space": return ref lot.square;
                case "rub": return ref lot.costPerMeter;
                case "summ": return ref lot.costWhole;
                case "bc": return ref lot.additionalInfo;
                default: throw new System.Exception("Failed to define lot field for prop name: " + propName);
            }
        }
    }
}