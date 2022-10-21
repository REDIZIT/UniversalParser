using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class CianDynamicParser : DynamicParser
    {
        private List<string> urls = new List<string>();

        private Thread watcherThread;
        private IInputField folder;
        //private IBrowser browser;
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
        private void Construct(IInputField folder/*, IBrowser browser*/)
        {
            this.folder = folder;
            //this.browser = browser;
            c = new();

            folder.Setup(new()
            {
                labelText = "Выгрузить таблицы в папку",
                placeholderText = "Путь до папки",
                validityCheckFunc = (s) => Directory.Exists(s)
            });
        }

        protected override void OnStart()
        {
            Debug.Log("On start");

            urls = urlsCreator.Create();

            var handler = new HttpClientHandler();
            Debug.Log("Disable proxy? " + File.Exists(Pathes.steamingAssets + "/noproxy.txt"));
            if (File.Exists(Pathes.steamingAssets + "/noproxy.txt") == false)
            {
                handler.Proxy = new WebProxy
                {
                    Address = new Uri("http://proxy.ko.wan:808"),
                    BypassProxyOnLocal = false
                };
            }

            HandleUrls();
        }

        protected override void OnStop()
        {
            watcherThread?.Abort();
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

                        Thread.Sleep(DELAY_SECONDS * 1000);
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
                //browser.Close();
            }
        }

        public void Dispose()
        {
            //browser?.Close();
            watcherThread?.Abort();
        }
    }
}