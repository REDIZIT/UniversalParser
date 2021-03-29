using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using UnityEngine;

namespace InGame.Parse
{
    public class ParseManager : MonoBehaviour
    {
        private Thread parseThread;


        private void Start()
        {
            //ParsePage("https://www.avito.ru/sankt-peterburg/kvartiry/prodam/vtorichka-ASgBAQICAUSSA8YQAUDmBxSMUg?cd=1&f=ASgBAQICAUSSA8YQAkDmBxSMUsoIpIpZmqwBmKwBlqwBlKwBiFmGWYRZglmAWQ&p=1");
        }
        public ParseProcess StartParsing(string url)
        {
            parseThread?.Abort();

            ParseProcess process = new ParseProcess();

            parseThread = new Thread(() => Parse(url, process));
            parseThread.Start();

            return process;
        }

        private void Parse(string url, ParseProcess process)
        {
            //try
            //{
                process.progress = 0;
                process.progressMessage = "Скачиваю страницу";


                process.result = ParsePage(url, process);


                process.state = ParseProcess.State.Finished;
                UnityMainThreadDispatcher.Instance().Enqueue(process.onfinished);
            //}
            //catch (Exception err)
            //{
            //    process.exception = err;
            //    process.state = ParseProcess.State.Finished;

            //    UnityMainThreadDispatcher.Instance().Enqueue(process.onfinished);
            //    Debug.LogError($"Parsing error for url: {url}.\nException is:\n{err}");
            //}
        }

        private ParseResult ParsePage(string url, ParseProcess process)
        {
            ParseResult result = new ParseResult();
            AvitoParser parser = new AvitoParser();

            HtmlDocument doc = parser.DownloadHtml(url);

            process.progress = 0.5f;
            process.progressMessage = "Анализирую";

            HtmlNode content = doc.DocumentNode.SelectSingleNode(".//div[@class='items-items-38oUm']");
            IEnumerable<HtmlNode> nodes = content.ChildNodes.Where(n => n.Attributes.Any(a => a.Name == "data-marker" && a.Value == "item"));
            result.lots.AddRange(parser.ParseLots(nodes));

            return result;
        }
    }
}