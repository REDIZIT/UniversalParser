using InGame.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;

namespace InGame.Dynamics
{
    public interface IBrowser
    {
        IWebDriver Driver { get; }
        void Open();
        void Close();
    }
    public class Yandex : IBrowser
    {
        public IWebDriver Driver { get; private set; }

        public void Open()
        {
            var service = ChromeDriverService.CreateDefaultService(Pathes.dataFolder + "/StreamingAssets", "yandexdriver.exe");
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
    }
}