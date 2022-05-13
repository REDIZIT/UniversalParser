using HtmlAgilityPack;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class SpbAfyParser : BrowserParser<MirkvartirLot>
    {
        public int CurrentPage { get; private set; }

        protected override string UrlPageArgument => "page";

        private HashSet<string> importedIDs = new HashSet<string>();
        private int pageCount = 0;

        public void SetImportResults(IEnumerable<string> IDs)
        {
            importedIDs.Clear();
            if (IDs != null) importedIDs.UnionWith(IDs);
        }
        public void SetPagesCount(int pageCount)
        {
            this.pageCount = pageCount;
        }
        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            HtmlDocument currentDocument = new HtmlDocument();

            bool isImportedNodeFound = false;
            CurrentPage = 0;

            while (isImportedNodeFound == false && (pageCount == 0 || CurrentPage < pageCount))
            {
                if (CurrentPage != 0)
                {
                    Debug.Log("Go to the next page");
                    try
                    {
                        driver.FindElement(By.ClassName("object-list-more-link")).Click();
                    }
                    catch
                    {
                        isImportedNodeFound = true;
                    }

                    Thread.Sleep(1000);
                }
                CurrentPage++;

                if (importedIDs.Count != 0)
                {
                    currentDocument.LoadHtml(driver.PageSource);

                    HtmlNode listNode = currentDocument.DocumentNode.SelectSingleNode(".//div[@class='object-list']").ChildNodes[1];

                    foreach (HtmlNode node in listNode.ChildNodes)
                    {
                        if (node.GetAttributeValue("data-role", null) == "pbbitem_in_list" && importedIDs.Contains(GetUrl(node)))
                        {
                            Debug.Log("Found imported lot");
                            isImportedNodeFound = true;
                            break;
                        }
                    }
                }
            }


            currentDocument.LoadHtml(driver.PageSource);
            foreach (HtmlNode node in currentDocument.DocumentNode.SelectSingleNode(".//div[@class='object-list']").ChildNodes[1].ChildNodes)
            {
                if (node.GetAttributeValue("data-role", null) == "pbbitem_in_list" && importedIDs.Contains(GetUrl(node)) == false)
                {
                    yield return node;
                }
            }
        }

        protected override MirkvartirLot ParseLotOrThrowException(HtmlNode node)
        {
            MirkvartirLot lot = new MirkvartirLot();


            lot.url = GetUrl(node);


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
        public override void OnParseFinished()
        {
            base.OnParseFinished();

            driver.Close();
        }
        private string GetUrl(HtmlNode lotNode)
        {
            HtmlNode urlButton = lotNode.SelectSingleNode(".//a[@class='object-item-info-more-link']");
            return urlButton.GetAttributeValue("href", null);
        }
    }
}