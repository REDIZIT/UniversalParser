using HtmlAgilityPack;
using System;
using System.Linq;
using UnityEngine;
using OpenQA.Selenium.Opera;
using System.IO;
using OpenQA.Selenium;

namespace UnityParser
{
    public abstract class BrowserParser<T> : Parser<T>, IParser where T : Lot
    {
        protected IWebDriver driver;

        public override HtmlDocument DownloadHtml(string url)
        {
            string filepath = Directory.GetFiles(Environment.CurrentDirectory, "operadriver.exe", SearchOption.AllDirectories).First();
            Debug.Log("Loading Opera driver from " + filepath);

            OperaDriverService service = OperaDriverService.CreateDefaultService(new FileInfo(filepath).DirectoryName);
            service.HideCommandPromptWindow = true;

            OperaOptions options = new OperaOptions();
            driver = new OperaDriver(service, options);

            driver.Navigate().GoToUrl(url);


            
            if (driver.Url.Contains("/login"))
            {
                Login(url);
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(driver.PageSource);

            return doc;
        }


        protected virtual void Login(string urlLoadAfterLogin)
        {
            throw new NotImplementedException();
        }
    }
}