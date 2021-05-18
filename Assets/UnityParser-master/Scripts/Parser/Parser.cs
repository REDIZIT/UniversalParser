using HtmlAgilityPack;
using InGame.Parse;
using InGame.Settings;
using RestSharp.Contrib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using UnityEngine;
using OpenQA.Selenium;

namespace UnityParser
{
    public interface IParser
    {
        ParseProcess process { get; }
        ParseProcess StartParsing(string url, int startPage = 0, int endPage = 0);
    }
    public abstract class Parser<T> : IParser where T : Lot
    {
        public ParseProcess process { get; protected set; }

        private Thread parseThread;

        public ParseProcess StartParsing(string url, int startPage = 0, int endPage = 0)
        {
            parseThread?.Abort();

            process = new ParseProcess();

            parseThread = new Thread(() => Parse(url, startPage, endPage));
            parseThread.Start();

            return process;
        }

		public virtual HtmlDocument DownloadHtml(string url)
        {
            HttpClientHandler handler = new HttpClientHandler();
            if (SettingsManager.settings.isProxyEnabled)
            {
                handler.Proxy = new WebProxy
                {
                    Address = new Uri(SettingsManager.settings.proxyAddress + ":" + SettingsManager.settings.proxyPort),
                    BypassProxyOnLocal = false
                };
            }

            using (HttpClient client = new HttpClient(handler))
            using (HttpResponseMessage resp = client.GetAsync(url).Result)
            {
                if (resp.IsSuccessStatusCode)
                {
                    string html = resp.Content.ReadAsStringAsync().Result;
                    if (string.IsNullOrEmpty(html) == false)
                    {
                        HtmlDocument doc = new HtmlDocument();
                        doc.LoadHtml(html);

                        return doc;
                    }
                }

                throw new Exception($"Webpage ({url}) can't be downloaded. Response code is {resp.StatusCode}");
            }
        }

        public IEnumerable<T> ParseLots(IEnumerable<HtmlNode> nodes)
        {
            foreach (HtmlNode node in nodes)
            {
                yield return ParseLot(node);
            }
        }
        public T ParseLot(HtmlNode node)
        {
            try
            {
                return ParseLotOrThrowException(node);
            }
            catch(Exception err)
            {
                T lot = Activator.CreateInstance<T>();
                lot.exception = err;

                Debug.LogError("Lot can't be handled." + "\n\nError is: " + err + "\n\nNode html was:\n " + node.InnerHtml);

                return lot;
            }
        }


        protected abstract IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc);
        protected abstract T ParseLotOrThrowException(HtmlNode node);


        private void Parse(string url, int startPage, int endPage)
        {
            if (startPage > 0 && endPage >= startPage)
            {
                var quary = HttpUtility.ParseQueryString(url);
                string baseUrl = url.Replace("p=" + quary["p"], "");

                if (baseUrl.Contains("?") == false)
                {
                    baseUrl += "?";
                }


                process.urlsToParse = new List<string>();
                for (int i = startPage; i <= endPage; i++)
                {
                    string urlToParse = baseUrl + "&p=" + i;
                    Debug.Log("urlToParse: " + urlToParse);
                    process.urlsToParse.Add(urlToParse);
                }
            }
            else
            {
                process.urlsToParse = new List<string>();
                process.urlsToParse.Add(url);
            }


            process.bigResult = new ParseResult();
            for (int i = 0; i < process.urlsToParse.Count; i++)
            {
                Parse(process.urlsToParse[i], i);
            }

            process.state = ParseProcess.State.Finished;
            UnityMainThreadDispatcher.Instance().Enqueue(process.onfinished);
        }
        private void Parse(string url, int currentUrlIndex)
        {
            process.progress = currentUrlIndex / (float)process.urlsToParse.Count;

            process.progressMessage = "Скачиваю страницу";


            ParseResult result = ParsePage(url, process);
            process.bigResult.lots.AddRange(result.lots);
            process.results.Add(result);
        }

        private ParseResult ParsePage(string url, ParseProcess process)
        {
            ParseResult result = new ParseResult();

            HtmlDocument doc = DownloadHtml(url);

            process.progress += 1 / (float)process.urlsToParse.Count / 2f;
            process.progressMessage = "Анализирую";

            IEnumerable<HtmlNode> nodesToParse = GetNodesToParse(doc);
            result.lots.AddRange(ParseLots(nodesToParse));

            return result;
        }
    }
}