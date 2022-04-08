using HtmlAgilityPack;
using System;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium;
using InGame;

namespace UnityParser
{
    public abstract class BrowserParser<T> : Parser<T>, IParser where T : Lot
    {
        protected IWebDriver driver;

        public override HtmlDocument DownloadHtml(string url)
        {
            OpenBrowser();

            throw new Exception();
            driver.Navigate().GoToUrl(url);


            
            if (driver.Url.Contains("/login"))
            {
                Login(url);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(driver.PageSource);

            driver.Close();

            return doc;
        }


        protected virtual void Login(string urlLoadAfterLogin)
        {
            throw new NotImplementedException();
        }

        private void OpenBrowser()
        {
            EdgeDriverService service = EdgeDriverService.CreateDefaultService(Pathes.dataFolder + "/StreamingAssets");
            service.HideCommandPromptWindow = true;

            EdgeOptions options = new EdgeOptions();

            driver = new EdgeDriver(service, options);
        }
    }
}