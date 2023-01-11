using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InGame.Dynamics
{
    public class SupportedBrowserElement : MonoBehaviour
    {
        [SerializeField] private Text versionText;
        [SerializeField] private Button updateButton;

        private WebClient c = new WebClient();

        [Inject] private IBrowser browser;

        private const string LATEST_URL = "https://api.github.com/repos/yandex/YandexDriver/releases";
        private const string DRIVER_INFO_PATH = "/driverAssetInfo.txt";

        [Serializable]
        private class Response
        {
            public Release[] items;
        }
        [Serializable]
        private class Release
        {
            public Asset[] assets;
        }
        [Serializable]
        private class Asset
        {
            public string BrowserVersion => name.Split('-')[1];

            public string name;
            public string browser_download_url;
        }

        private void Start()
        {
            UpdateVersionText();
        }
        public void OnUpdateClicked()
        {
            StartCoroutine(IEStartUpdate());
        }
        private IEnumerator IEStartUpdate()
        {
            updateButton.interactable = false;
            yield return null;

            Debug.Log("Closing browser");
            browser.Close();

            Asset winAsset = GetLatestAsset();

            Debug.Log("Downloading asset " + winAsset.name + " from " + winAsset.browser_download_url);

            string zipFile = Pathes.steamingAssets + "/temp.zip";
            string infoFile = Pathes.steamingAssets + DRIVER_INFO_PATH;


            c.Headers.Add("User-Agent", "request");
            c.DownloadFile(winAsset.browser_download_url, zipFile);
            ZipFile.ExtractToDirectory(zipFile, Pathes.steamingAssets, true);

            File.WriteAllText(infoFile, winAsset.BrowserVersion);
            File.Delete(zipFile);


            UpdateVersionText();
            updateButton.interactable = true;
        }

        private void UpdateVersionText()
        {
            StringBuilder b = new StringBuilder();

            string format = "<color=#efc8a4><size=12>";
            string formatEnd = "</size></color>";

            b.AppendLine($"Версия Chromium: {format}загрузка{formatEnd}");
            b.Append($"Парсер поддерживает версию браузера: {format}загрузка{formatEnd}");
            versionText.text = b.ToString();
            updateButton.gameObject.SetActive(false);

            b.Clear();

            Task t = Task.Run(() =>
            {
                b.AppendLine($"Версия Chromium: {format}{GetDriverVersion()}{formatEnd}");

                string currentVersion = File.ReadAllText(Pathes.steamingAssets + DRIVER_INFO_PATH);
                b.Append($"Парсер поддерживает версию браузера: {format}{currentVersion}{formatEnd}");


                string latestVersion = GetLatestAsset().BrowserVersion;
                if (latestVersion != currentVersion)
                {
                    b.AppendLine($"\n\n<size=12><color=#efc8a4>Доступно обновление драйвера <size=10>({latestVersion})</size>. Обновите драйвер если после обновления браузера парсер перестал работать.</color></size>");
                    b.AppendLine();
                }

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    updateButton.gameObject.SetActive(latestVersion != currentVersion);
                    versionText.text = b.ToString();
                });
            });
        }
        private string GetDriverVersion()
        {
            System.Diagnostics.Process process = new();

            process.StartInfo.FileName = Pathes.steamingAssets + "/yandexdriver.exe";
            process.StartInfo.Arguments = "-v";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            return process.StandardOutput.ReadToEnd().Split()[1];
        }
        private Asset GetLatestAsset()
        {
            c.Headers.Add("User-Agent", "request");
            string json = c.DownloadString(LATEST_URL);

            var releases = JsonConvert.DeserializeObject<Release[]>(json);

            foreach (Release release in releases)
            {
                Asset winAsset = release.assets.FirstOrDefault(c => c.name.ToLower().Contains("win"));
                if (winAsset != null)
                {
                    return winAsset;
                }
            }

            throw new Exception("No any asset found for releases: " + json);
        }
    }
}