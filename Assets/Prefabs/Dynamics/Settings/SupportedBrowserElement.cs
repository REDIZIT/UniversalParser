using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public class SupportedBrowserElement : MonoBehaviour
    {
        [SerializeField] private Text versionText;
        [SerializeField] private Button updateButton;

        private WebClient c;

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
            c = new WebClient();
            c.Headers.Add("User-Agent", "request");

            UpdateVersionText();
        }
        public void OnUpdateClicked()
        {
            Asset winAsset = GetLatestAsset();

            Debug.Log("Downloading asset " + winAsset.name + " from " + winAsset.browser_download_url);

            string zipFile = Pathes.steamingAssets + "/temp.zip";
            string infoFile = Pathes.steamingAssets + DRIVER_INFO_PATH;


            c.DownloadFile(winAsset.browser_download_url, zipFile);
            ZipFile.ExtractToDirectory(zipFile, Pathes.steamingAssets, true);

            File.WriteAllText(infoFile, winAsset.BrowserVersion);
            File.Delete(zipFile);


            UpdateVersionText();
        }

        private void UpdateVersionText()
        {
            StringBuilder b = new StringBuilder();

            string format = "<color=#efc8a4><size=12>";
            string formatEnd = "</size></color>";

            b.AppendLine($"Версия Chromium: {format}{GetDriverVersion()}{formatEnd}");

            string currentVersion = File.ReadAllText(Pathes.steamingAssets + DRIVER_INFO_PATH);
            b.AppendLine($"Парсер поддерживает версию браузера: {format}{currentVersion}{formatEnd}");


            string latestVersion = GetLatestAsset().BrowserVersion;
            if (latestVersion != currentVersion)
            {
                b.AppendLine($"\r\n<size=12><color=#efc8a4>Доступно обновление драйвера <size=10>({latestVersion})</size>. Обновите драйвер если после обновления браузера парсер перестал работать.</color></size>");
            }

            versionText.text = b.ToString().Trim();
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