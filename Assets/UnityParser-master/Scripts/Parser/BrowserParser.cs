using HtmlAgilityPack;
using System;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using InGame;

namespace UnityParser
{
    public abstract class BrowserParser<T> : Parser<T>, IParser where T : Lot
    {
        protected IWebDriver driver;

        public override HtmlDocument DownloadHtml(string url)
        {
            if (driver == null)
            {
                OpenBrowser();
            }

            driver.Navigate().GoToUrl(url);


            
            if (driver.Url.Contains("/login"))
            {
                Login(url);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(driver.PageSource);

            //driver.Close();

            return doc;
        }
        //~BrowserParser()
        //{
        //    driver?.Close();
        //    driver?.Quit();
        //    driver?.Dispose();
        //}

        protected virtual void Login(string urlLoadAfterLogin)
        {
            throw new NotImplementedException();
        }

        private void OpenBrowser()
        {
            var service = ChromeDriverService.CreateDefaultService(Pathes.dataFolder + "/StreamingAssets");
            service.HideCommandPromptWindow = true;

            driver = new ChromeDriver(service);
        }
    }
}