using HtmlAgilityPack;
using InGame.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace InGame.Dynamics
{
    public class Yandex : IBrowser
    {
        private IWebDriver Driver;
        private ChromeDriverService service;

        public void Open()
        {
            service = ChromeDriverService.CreateDefaultService(Pathes.steamingAssets, "yandexdriver.exe");
            service.HideCommandPromptWindow = false;

            Dictionary<string, object> chromePrefs = new();
            chromePrefs.Add("profile.default_content_settings.popups", 0);
            ChromeOptions options = new ChromeOptions();

            options.AddUserProfilePreference("prefs", chromePrefs);
            options.AddArgument("download.default_directory=C:/Users/REDIZIT/Desktop/1");

            if (SettingsManager.settings.enableImageLoading == false)
            {
                options.AddUserProfilePreference("profile.managed_default_content_settings.images", 2);
            }

            Driver = new ChromeDriver(service, options);
        }
        public void Close()
        {
            Debug.Log("Yandex close");

            Driver?.Close();
            Driver?.Quit();
            Driver?.Dispose();
            Driver = null;

            service?.Dispose();
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
        }
    }
}