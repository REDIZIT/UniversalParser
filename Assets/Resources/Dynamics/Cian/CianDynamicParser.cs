using InGame.Settings;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class CianDynamicParser : DynamicParser
    {
        private List<string> urls = new List<string>();

        private IOption option;
        private IInputField folder;
        private WebClient c;
        private float chillTimeLeft;

        private readonly UrlsCreator urlsCreator = new();

        /// <summary>Delay between requests (in seconds)</summary>
        public const int DELAY_SECONDS = 1;
        /// <summary>Count of urls to check before have a chill</summary>
        public const int CHILL_URLS_COUNT = 100;
        /// <summary>Chill time in seconds</summary>
        public const int CHILL_DELAY = 30;

        [Inject]
        private void Construct(IOption option, IInputField folder)
        {
            this.option = option;
            this.folder = folder;
            c = new WebClient();

            if (SettingsManager.settings.isProxyEnabled)
            {
                c.Proxy = new WebProxy
                {
                    Address = new Uri(SettingsManager.settings.proxyAddress + ":" + SettingsManager.settings.proxyPort),
                    BypassProxyOnLocal = false
                };
            }

            option.Setup(new IOption.Model()
            {
                title = "Выберите макет выгрузки",
                items = new List<IOption.Item>()
                {
                    new IOption.Item() { text = "Вторичка (район + площади)", value = UrlsCreator.Type.SaleDistrictArea },
                    new IOption.Item() { text = "Аренда (район + площади)", value = UrlsCreator.Type.RentDistrictArea },
                    new IOption.Item() { text = "Комнаты (только район)", value = UrlsCreator.Type.SaleRoomsDistrict },
                    new IOption.Item() { text = "Аппартаменты (площади по новым правилам)", value = UrlsCreator.Type.SaleApartmentsSpecial },
                }
            });
            folder.Setup(new()
            {
                labelText = "Выгрузить таблицы в папку",
                placeholderText = "Путь до папки",
                validityCheckFunc = (s) => Directory.Exists(s)
            });

            BakeElements();
        }

        protected override void OnStart()
        {
            urls = urlsCreator.Create((UrlsCreator.Type)option.Selected.value);

            HandleUrls();
        }

        protected override void OnStop()
        {
            status.Status = "Stopped";
        }
        public void HandleUrls()
        {
            try
            {
                status.Status = "Скачивание";
                status.Progress = "0 / " + urls.Count;

                for (int i = 1; i <= urls.Count; i++)
                {
                    status.Progress = i + " / " + urls.Count;

                    string url = urls[i - 1];

                    try
                    {
                        // Export url
                        // https://spb.cian.ru/export/xls/offers/?deal_type=sale&district%5B0%5D=747&engine_version=2&object_type%5B0%5D=1&offer_type=flat&room7=1&room9=1&totime=864000
                        // Second url, rent: https://spb.cian.ru/cat.php?deal_type=rent&engine_version=2&offer_type=flat&only_flat=1&region=2&room1=1&totime=604800&type=4
                        // Thrird url, with another rules: https://spb.cian.ru/cat.php?deal_type=sale&district%5B0%5D=747&engine_version=2&object_type%5B0%5D=1&offer_type=flat&room1=1&totime=864000
                        //url = url.Replace("cat.php", "export/xls/offers");

                        if (i % CHILL_URLS_COUNT == 0 && i > 0)
                        {
                            chillTimeLeft = CHILL_DELAY;
                            while (chillTimeLeft > 0)
                            {
                                status.Status = "Пауза (" + (int)chillTimeLeft + "с)";
                                Thread.Sleep(1000);
                                chillTimeLeft--;
                            }
                        }

                        status.Status = "Скачивание";

                        string downloadUrl = url.Replace("cat.php", "export/xls/offers/");

                        string targetFileName = folder.Text + "/" + (Directory.GetFiles(folder.Text).Length + 1) + ".xlsx";
                        c.DownloadFile(downloadUrl, targetFileName);

                        Thread.Sleep(DELAY_SECONDS * 1250);
                    }
                    catch (Exception err)
                    {
                        if (err is ThreadAbortException == false)
                        {
                            if (err is WebDriverTimeoutException || err is TimeoutException)
                            {
                                Debug.LogError($"[URL HANDLE ERROR] Work will continue. '{url}' threw exception: " + err);
                            }
                            else
                            {
                                Debug.LogError($"[URL HANDLE ERROR] Work will be stopped. '{url}' threw exception: " + err);
                                throw;
                            }
                        }
                    }
                }

                Thread.Sleep(4000);
                status.Status = "Done";
            }
            catch (Exception err)
            {
                if (err is ThreadAbortException)
                {
                    status.Status = "Idle";
                    return;
                }
                status.Status = "Error";
                Debug.LogException(err);

            }
            finally
            {
                status.Status = "Done at " + DateTime.Now.TimeOfDay;
            }
        }
    }
}