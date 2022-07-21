using HtmlAgilityPack;
using InGame.UI;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class OstrovokParser : BrowserParser<LotContainer<OstrovokLot>>
    {
        protected override string UrlPageArgument => "page";
        private SelectTableControl selectTableControl;

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
                if (tryCount >= 20) throw new System.TimeoutException("OstrovokParser page load timeout. Not found div with class hotels-inner");
                Thread.Sleep(1000);
            }

            Thread.Sleep(10000);

            return doc.DocumentNode.SelectNodes(".//a[@class='zen-hotelcard-name-link link']");
        }

        protected override LotContainer<OstrovokLot> ParseLotOrThrowException(HtmlNode node)
        {
            string url = "https://ostrovok.ru" + node.GetAttributeValue("href", "");

            driver.Navigate().GoToUrl(url);
            Thread.Sleep(1000);

            LotContainer<OstrovokLot> container = new LotContainer<OstrovokLot>();
            string hotelName = driver.FindElement(By.ClassName("zen-roomspage-title-name")).Text;
            string hotelAddress = driver.FindElement(By.ClassName("zenroomspagelocation-address")).Text;

            container.url = hotelName;

            foreach (var room in driver.FindElements(By.ClassName("zen-roomspagerooms-room")))
            {
                string roomName = room.FindElement(By.ClassName("zenroomspagerate-name-title")).Text.Replace(@"
", ", ");

                OstrovokLot lot = new OstrovokLot(hotelName + ";" + roomName);
                container.lots.Add(lot);

                lot.hotelName = hotelName;
                lot.hotelAddress = hotelAddress;
                lot.roomName = roomName;

                lot.food = room.FindElement(By.ClassName("valueadds-item-meal")).Text.Replace("?", "");
                lot.cancelPrice = room.FindElement(By.ClassName("valueadds-item-cancellation")).Text.Replace("?", "");
                lot.payMethod = room.FindElement(By.ClassName("valueadds-item-payment")).Text.Replace("?", "");

                string priceStr = room.FindElement(By.ClassName("zenroomspage-b2c-rates-price-amount")).Text.Replace("?", "");
                string taxStr = (TryGetText(room, By.ClassName("zenroomspage-b2c-rates-price-included")) + TryGetText(room, By.ClassName("zenroomspage-b2c-rates-price-postpay"))).Replace("?", "");
                lot.price = $"{priceStr} ({taxStr})";

                try
                {
                    lot.square = room.FindElement(By.ClassName("zenroomspageroom-header-content-amenity-square")).Text.Split(' ')[0];
                }
                catch { }
            }

            return container;
        }

        public void Setup(SelectTableControl selectTableControl)
        {
            this.selectTableControl = selectTableControl;
        }
        public override void OnParseFinished()
        {
            Debug.Log("OstrovokParser OnParseFinished photo save disabled");
            return;

            Debug.Log("OnParseFinished, photosCount: " + process.results.Sum(s => s.EnumerateUnpackedLots().Cast<OstrovokLot>().Sum(l => l.photos.Count)));
            FileInfo fileInfo = new FileInfo(selectTableControl.tableFilePath);
            string photoFolder = fileInfo.Directory.FullName.Replace(@"\\", "/") + "/Фотографии";
            Directory.CreateDirectory(photoFolder);

            WebClient c = new WebClient();

            foreach (OstrovokLot lot in process.results.SelectMany(t => t.EnumerateUnpackedLots().Cast<OstrovokLot>()))
            {
                string lotPhotoFolder = photoFolder + "/" + lot.hotelName + "/" + lot.roomName;
                Directory.CreateDirectory(lotPhotoFolder);

                int i = -1;
                foreach (string photoUrl in lot.photos)
                {
                    i++;
                    c.DownloadFile(photoUrl, lotPhotoFolder + "/" + i + ".jpg");
                }
            }
        }
    }

    public class OstrovokLot : Lot
    {
        [ExcelString("Название отеля")]
        public string hotelName;

        [ExcelString("Адрес отеля")]
        public string hotelAddress;

        [ExcelString("Название номера")]
        public string roomName;

        [ExcelString("Площадь")]
        public string square;

        [ExcelString("Питание")]
        public string food;

        [ExcelString("Отмена")]
        public string cancelPrice;

        [ExcelString("Оплата")]
        public string payMethod;

        [ExcelString("Цена")]
        public string price;

        public List<string> photos = new List<string>();

        public OstrovokLot() : base() { }
        public OstrovokLot(string url) : base(url) { }
    }
}