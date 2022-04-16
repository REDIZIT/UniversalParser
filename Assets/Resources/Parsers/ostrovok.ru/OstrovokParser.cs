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

            Thread.Sleep(3000);

            return doc.DocumentNode.SelectNodes(".//a[@class='zen-hotelcard-name-link link']");
        }

        protected override LotContainer<OstrovokLot> ParseLotOrThrowException(HtmlNode node)
        {
            string url = "https://ostrovok.ru" + node.GetAttributeValue("href", "");

            driver.Navigate().GoToUrl(url);
            Thread.Sleep(1000);

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

                lot.food = room.FindElement(By.ClassName("valueadds-item-title-inner")).Text.Replace("?", "");
                lot.cancelPrice = room.FindElement(By.ClassName("valueadds-item-cancellation")).Text.Replace("?", "");
                lot.payMethod = room.FindElement(By.ClassName("valueadds-item-payment")).Text.Replace("?", "");
                lot.price = room.FindElement(By.ClassName("zenroomspage-b2c-rates-price-amount")).Text.Replace("?", "");
            }

            return container;
        }
    }

    public class OstrovokLot : Lot
    {
        [ExcelString("Название отеля")]
        public string hotelName;

        [ExcelString("Название номера")]
        public string roomName;

        [ExcelString("Питание")]
        public string food;

        [ExcelString("Отмена")]
        public string cancelPrice;

        [ExcelString("Оплата")]
        public string payMethod;

        [ExcelString("Цена")]
        public string price;

        public OstrovokLot() : base() { }
        public OstrovokLot(string url) : base(url) { }
    }
}