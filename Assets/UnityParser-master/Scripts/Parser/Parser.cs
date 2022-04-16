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
using System.Linq;
using InGame.Exceptions;
using InGame.UI;

namespace UnityParser
{
    public interface IParser
    {
        ParseProcess process { get; }
        ParseProcess StartParsing(string url, int startPage = 0, int endPage = 0);
        int GetCurrentPageNumberByUrl(string url);
        string GetUrlWithPageNumber(string baseUrl, int pageNumber);
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

                throw new ExceptionResponseCode($"Webpage ({url}) can't be downloaded. Response code is {resp.StatusCode}", resp.StatusCode);
            }
        }
        public virtual void OnParseFinished() { }

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
                Debug.LogException(err);

                return lot;
            }
        }
        public int GetCurrentPageNumberByUrl(string url)
        {
            var quary = HttpUtility.ParseQueryString(url);
            if (string.IsNullOrEmpty(quary[UrlPageArgument]))
            {
                return -1;
            }

            return int.Parse(quary[UrlPageArgument]);
        }
        public string GetUrlWithPageNumber(string baseUrl, int pageNumber)
        {
            var quary = HttpUtility.ParseQueryString(baseUrl);
            baseUrl = baseUrl.Replace(UrlPageArgument + "=" + quary[UrlPageArgument], "");

            if (baseUrl.Contains("?") == false)
            {
                baseUrl += "?";
            }

            if (baseUrl.EndsWith("&"))
            {
                return baseUrl + UrlPageArgument + "=" + pageNumber;
            }
            else 
            {
                return baseUrl + "&" + UrlPageArgument + "=" + pageNumber;
            }
        }


        protected abstract IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc);
        protected abstract T ParseLotOrThrowException(HtmlNode node);
        protected abstract string UrlPageArgument { get; }

        private void Parse(string url, int startPage, int endPage)
        {
            if (startPage > 0 && endPage >= startPage)
            {
                var quary = HttpUtility.ParseQueryString(url);
                string baseUrl = url.Replace(UrlPageArgument + "=" + quary[UrlPageArgument], "");

                if (baseUrl.Contains("?") == false)
                {
                    baseUrl += "?";
                }


                process.urlsToParse = new List<string>();
                for (int i = startPage; i <= endPage; i++)
                {
                    string urlToParse = baseUrl + "&" + UrlPageArgument + "=" + i;
                    process.urlsToParse.Add(urlToParse);
                }
            }
            else
            {
                process.urlsToParse = new List<string>();
                process.urlsToParse.Add(url);
            }


            process.bigResult = new ParseResult<T>();
            for (int i = 0; i < process.urlsToParse.Count; i++)
            {
                Parse(process.urlsToParse[i], i);
                UnityMainThreadDispatcher.Instance().Enqueue(process.onPageParsed);
            }

            process.state = ParseProcess.State.Finished;
            UnityMainThreadDispatcher.Instance().Enqueue(process.onfinished);

            OnParseFinished();
        }
        private void Parse(string url, int currentUrlIndex)
        {
            process.progress = currentUrlIndex / (float)process.urlsToParse.Count;

            process.progressMessage = "Скачиваю страницу";


            ParseResult<T> result = ParsePage(url, process);
            process.bigResult.AddRange(result.lots);
            process.results.Add(result);
            process.currentPageResult = result;
        }

        private ParseResult<T> ParsePage(string url, ParseProcess process)
        {
            ParseResult<T> result = new ParseResult<T>();

            HtmlDocument doc = null;
            try
            {
                doc = DownloadHtml(url);
            }
            catch (Exception err)
            {
                process.progressMessage = "Неизвестная ошибка во время скачивания страницы (консоль)";

                if (err is ExceptionResponseCode resp)
                {
                    string code = resp.responseCode.ToString();
                    if ((int)resp.responseCode == 429)
                    {
                        code = "Слишком много запросов";
                    }

                    process.progressMessage = $"Ошибка во время скачивания страницы ({code})";
                }

                if (err.Message.Contains("This version of MSEdgeDriver only supports MSEdge"))
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => GlobalUI.legacyDriverWindow.Show(err));
                }
                throw err;
            }

            process.progress += 1 / (float)process.urlsToParse.Count / 2f;
            process.progressMessage = "Анализирую";

            IEnumerable<HtmlNode> nodesToParse = GetNodesToParse(doc);
            result.lots.AddRange(ParseLots(nodesToParse));

            return result;
        }
    }
}