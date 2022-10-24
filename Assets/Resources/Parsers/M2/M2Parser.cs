using HtmlAgilityPack;
using InGame.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using RestSharp.Contrib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class M2Parser : BrowserParser<MirkvartirLot>
    {
        protected override string UrlPageArgument => "pageNumber";

        protected override void Login(string urlLoadAfterLogin)
        {
            var rnd = new System.Random();

            IWebElement phone = driver.FindElement(By.Name("phone"));
            phone.SendKeys(SettingsManager.settings.m2Login);
            phone.Submit();


            var timeoutDate = DateTime.Now + TimeSpan.FromSeconds(10);
            bool successPasswordTyped = false;

            while (DateTime.Now < timeoutDate)
            {
                Thread.Sleep(100);

                try
                {
                    IWebElement password = driver.FindElement(By.Name("password"));
                    password.SendKeys(SettingsManager.settings.m2Password);
                    password.Submit();

                    successPasswordTyped = true;
                    break;
                }
                catch { }
            }

            if (successPasswordTyped == false)
            {
                throw new Exception("Timeout. Can't enter password into IWebElement.");
            }


            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            wait.Until(d =>
            {
                try
                {
                    d.FindElement(By.ClassName("print-listing"));
                    return true;
                }
                catch
                {
                    return false;
                }
            });
        }

        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            HtmlNode listNode = doc.DocumentNode.SelectSingleNode(".//div[@class='print-listing']");
            if (listNode == null)
            {
                listNode = doc.DocumentNode.SelectSingleNode(".//ul[@data-test='offers']");
            }

            foreach (HtmlNode node in listNode.ChildNodes)
            {
                if (node.HasClass("ProfOfferSnippet") || node.FirstChild.GetAttributeValue("data-test", null) == "offer")
                {
                    yield return node;
                }
            }
        }

        protected override MirkvartirLot ParseLotOrThrowException(HtmlNode node)
        {

            HtmlNode urlButton = node.SelectSingleNode(".//a[@data-gtm-zbs='snippet-open-window']");
            if (urlButton != null)
            {
                return ParseProfessionalSearch(node);
            }
            else
            {
                return ParseRegularSearch(node);
            }
        }

        private MirkvartirLot ParseProfessionalSearch(HtmlNode node)
        {
            HtmlNode urlButton = node.SelectSingleNode(".//a[@data-gtm-zbs='snippet-open-window']");
            string url = urlButton.GetAttributeValue("href", "<url not found>");

            MirkvartirLot lot = new MirkvartirLot(@"https://m2.ru/" + url);

            var group = node.SelectSingleNode(".//div[@class='CharacteristicsGroup']");

            lot.rooms = group.ChildNodes[0].InnerText;
            lot.area = group.ChildNodes[1].InnerText;
            lot.floor = group.ChildNodes[2].InnerText;


            lot.price = HttpUtility.HtmlDecode(node.SelectSingleNode(".//div[@class='PriceHistory__trigger']").InnerText);


            var metroNode = node.SelectSingleNode(".//div[@class='NewMetroStationTitle__title']");
            if (metroNode != null)
            {
                lot.metroOrDistrict = metroNode.InnerText;
            }

            lot.address = node.SelectSingleNode(".//div[@data-test='offer-snippet-address']").InnerText;

            return lot;
        }
        private MirkvartirLot ParseRegularSearch(HtmlNode node)
        {
            HtmlNode urlButton = node.SelectSingleNode(".//div[@data-test='offer-title']").FirstChild;
            string url = urlButton.GetAttributeValue("href", "<url not found>");

            MirkvartirLot lot = new MirkvartirLot(@"https://m2.ru/" + url);


            string titleText = HttpUtility.HtmlDecode(urlButton.InnerText);
            Recognition.Recognizer.TrySplit(titleText, out var result);

            lot.rooms = result.name;
            lot.area = result.area;
            lot.floor = result.storeys;


            lot.price = HttpUtility.HtmlDecode(node.SelectSingleNode(".//span[@itemprop='price']").InnerText);


            var metroNode = node.SelectSingleNode(".//a[@class='SubwayStation__link']");
            if (metroNode != null)
            {
                lot.metroOrDistrict = metroNode.InnerText;
            }

            lot.address = node.SelectSingleNode(".//div[@class='ClClickableAddress__links']").InnerText;

            return lot;
        }
    }
}