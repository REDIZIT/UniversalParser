using InGame.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

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

            ChromeOptions options = new ChromeOptions();

            if (SettingsManager.settings.enableImageLoading == false)
            {
                options.AddUserProfilePreference("profile.managed_default_content_settings.images", 2);
            }

            Driver = new ChromeDriver(service, options);
        }
        public void Close()
        {
            Driver?.Quit();
            Driver?.Close();
            Driver?.Dispose();
            Driver = null;
        }
    }
}