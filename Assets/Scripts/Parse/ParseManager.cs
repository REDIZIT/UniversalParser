using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace InGame.Parse
{
    public class ParseManager : MonoBehaviour
    {
        private Thread parseThread;

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
            process.progress = 0;
            process.progressMessage = "Скачиваю страницу";


            process.result = ParsePage(url, process);


            process.state = ParseProcess.State.Finished;
            UnityMainThreadDispatcher.Instance().Enqueue(process.onfinished);
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