using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using UnityEngine;

namespace UnityParser
{
    public abstract class Parser<T> where T : Lot
	{
		public HtmlDocument DownloadHtml(string url)
        {
            // First create a proxy object
            WebProxy proxy = new WebProxy
            {
                Address = new Uri($"http://proxy.ko.wan:808"),
                BypassProxyOnLocal = false
            };


            using (HttpClientHandler handler = new HttpClientHandler()/* { Proxy = proxy }*/)
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
	}

	public class Lot
    {
        public Exception exception;
    }
}