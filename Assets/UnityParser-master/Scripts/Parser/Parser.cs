using HtmlAgilityPack;
using InGame.Parse;
using InGame.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using UnityEngine;

namespace UnityParser
{
    public interface IParser
    {
        ParseProcess process { get; }
        ParseProcess StartParsing(string url);
    }
    public abstract class Parser<T> : IParser where T : Lot
    {
        public ParseProcess process { get; protected set; }

        private Thread parseThread;

        public ParseProcess StartParsing(string url)
        {
            parseThread?.Abort();

            process = new ParseProcess();

            parseThread = new Thread(() => Parse(url));
            parseThread.Start();

            return process;
        }

		public HtmlDocument DownloadHtml(string url)
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

                Debug.LogError("Lot can't be handled.\n\nNode html was:\n " + node.InnerHtml + "\n\nError is: " + err);

                return lot;
            }
        }


        protected abstract T ParseLotOrThrowException(HtmlNode node);


        private void Parse(string url)
        {
            process.progress = 0;
            process.progressMessage = "Скачиваю страницу";


            process.result = ParsePage(url, process);


            process.state = ParseProcess.State.Finished;
            UnityMainThreadDispatcher.Instance().Enqueue(process.onfinished);
        }
        private ParseResult ParsePage(string url, ParseProcess process)
        {
            ParseResult result = new ParseResult();

            HtmlDocument doc = DownloadHtml(url);

            process.progress = 0.5f;
            process.progressMessage = "Анализирую";

            HtmlNode content = doc.DocumentNode.SelectSingleNode(".//div[@class='items-items-38oUm']");
            IEnumerable<HtmlNode> nodes = content.ChildNodes.Where(n => n.Attributes.Any(a => a.Name == "data-marker" && a.Value == "item"));
            result.lots.AddRange(ParseLots(nodes));

            return result;
        }
    }

	public class Lot
    {
        public Exception exception;

        [ExcelID]
        [ExcelString("Ссылка", 1200)]
        public string url;
    }
}