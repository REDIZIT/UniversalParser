using HtmlAgilityPack;
using InGame.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace InGame.Dynamics
{
    public interface IBrowser
    {
        void Open();
        void Close();
        void GoToUrl(string url);
        void GetDocument(HtmlDocument documentToUpdate);
    }
    public class FakeBrowser : IBrowser
    {
        public void Open() { }
        public void Close() { }

        private int getNumber = 0;
        private readonly string[] html;

        public FakeBrowser(string[] html)
        {
            this.html = html;
            Debug.Log("Fake browser has been loaded with " + html.Length + " documents");
        }
        public void GoToUrl(string url) { }
        public void GetDocument(HtmlDocument doc)
        {
            Debug.Log("Get fake document: " + getNumber);
            doc.LoadHtml(html[getNumber]);
            getNumber++;
        }
    }
    public class Yandex : IBrowser
    {
        private IWebDriver Driver;
        private int i;

        public void Open()
        {
            var service = ChromeDriverService.CreateDefaultService(Pathes.steamingAssets, "yandexdriver.exe");
            service.HideCommandPromptWindow = true;

            string downloadFilepath = @"C:\\Users\\REDIZIT\\Desktop\\1";
            Dictionary<string, object> chromePrefs = new();
            chromePrefs.Add("profile.default_content_settings.popups", 0);
            chromePrefs.Add("download.default_directory", downloadFilepath);
            ChromeOptions options = new ChromeOptions();

            options.AddUserProfilePreference("prefs", chromePrefs);

            if (SettingsManager.settings.enableImageLoading == false)
            {
                options.AddUserProfilePreference("profile.managed_default_content_settings.images", 2);
            }

            Driver = new ChromeDriver(service, options);
        }
        public void Close()
        {
            Driver?.Quit();
            Driver?.Dispose();
            Driver = null;
        }
        public void GoToUrl(string url)
        {
            Debug.Log("GoToUrl " + url);
            Driver.Navigate().GoToUrl(url);
        }
        public void GetDocument(HtmlDocument documentToUpdate)
        {
            string html = Driver.PageSource;

            documentToUpdate.LoadHtml(html);

            i++;
            File.WriteAllText("C:\\Users\\REDIZIT\\Documents\\GitHub\\UniversalParser\\Assets\\UnitTests\\EditMode\\AvitoTest\\doc_" + i + ".html", html);
        }
    }
}