using HtmlAgilityPack;
using InGame.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Threading;
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

            Driver = new ChromeDriverEx(service, options);
        }
        public void Close()
        {
            Driver?.Close();
            Driver?.Quit();
            Driver?.Dispose();
            Driver = null;

            service?.Dispose();
        }
        public void GoToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
        }
        public void GetDocument(HtmlDocument documentToUpdate)
        {
            string html = Driver.PageSource;

            documentToUpdate.LoadHtml(html);
        }
        public void Maximize()
        {
            Driver.Manage().Window.Maximize();
        }

        public void Screenshot()
        {
            var screenshot = ((ChromeDriver)Driver).GetScreenshot();
            screenshot.SaveAsFile("C:\\Users\\REDIZIT\\Desktop\\1\\123.png", ScreenshotImageFormat.Png);
        }

        public void ScreenshotFullPage(string filepath)
        {
            var driver = (ChromeDriverEx) Driver;

            var screenshot = driver.GetFullPageScreenshot();
            screenshot.SaveAsFile(filepath, ScreenshotImageFormat.Png);
        }
    }
}