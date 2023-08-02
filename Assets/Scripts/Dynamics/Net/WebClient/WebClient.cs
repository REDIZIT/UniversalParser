using HtmlAgilityPack;
using InGame.Settings;
using System;
using System.Net;
using UnityEngine;

namespace InGame.Dynamics
{
    public interface IWebClient
    {
        void Download(string url, HtmlDocument documentToUpdate);
        void DownloadFile(string url, string fileName);
    }
    public class WebClientWrapper : IWebClient
    {
        private readonly WebClient client;

        public WebClientWrapper()
        {
            Debug.Log("WebClient ctor");

            client = new WebClient();
            if (SettingsManager.settings.isProxyEnabled)
            {
                client.Proxy = new WebProxy
                {
                    Address = new Uri(SettingsManager.settings.proxyAddress + ":" + SettingsManager.settings.proxyPort),
                    BypassProxyOnLocal = false
                };
            }
        }

        public void Download(string url, HtmlDocument documentToUpdate)
        {
            documentToUpdate.LoadHtml(client.DownloadString(url));
        }
        public void DownloadFile(string url, string fileName)
        {
            client.DownloadFile(url, fileName);
        }
    }
}