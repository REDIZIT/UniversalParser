using HtmlAgilityPack;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class OstrovokParser : BrowserParser<LotContainer<OstrovokLot>>
    {
        protected override string UrlPageArgument => "page";

        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            int tryCount = 0;
            while (true)
            {
                doc.LoadHtml(driver.PageSource);

                if (driver.FindElements(By.ClassName("hotels-inner")).Count != 0)
                {
                    break;
                }

                tryCount++;
                if (tryCount >= 10) throw new System.TimeoutException("OstrovokParser page load timeout. Not found div with class hotels-inner");
                Thread.Sleep(1000);
            }

            HtmlNode link = doc.DocumentNode.SelectSingleNode(".//a[@class='zen-hotelcard-name-link link']");
            yield return link;
        }

        protected override LotContainer<OstrovokLot> ParseLotOrThrowException(HtmlNode node)
        {
            string url = "https://ostrovok.ru" + node.GetAttributeValue("href", "");

            driver.Navigate().GoToUrl(url);
            LotContainer<OstrovokLot> container = new LotContainer<OstrovokLot>();
            string hotelName = driver.FindElement(By.ClassName("zen-roomspage-title-name")).Text;

            container.url = hotelName;

            foreach (var room in driver.FindElements(By.ClassName("zen-roomspagerooms-room")))
            {
                string roomName = room.FindElement(By.ClassName("zenroomspagerate-name-title")).Text;

                OstrovokLot lot = new OstrovokLot(hotelName + ";" + roomName);
                container.lots.Add(lot);

                lot.hotelName = hotelName;
                lot.roomName = roomName;
            }

            Debug.Log("Container lots count: " + container.lots.Count);
            return container;
        }
    }

    public class OstrovokLot : Lot
    {
        [ExcelString("Название отеля")]
        public string hotelName;

        [ExcelString("Название номера")]
        public string roomName;

        [ExcelID()]
        [ExcelString("ID")]
        public new string url;

        public OstrovokLot() : base() { }
        public OstrovokLot(string url) : base(url)
        {
            this.url = url;
        }
    }
}