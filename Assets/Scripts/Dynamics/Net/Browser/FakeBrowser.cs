using HtmlAgilityPack;
using UnityEngine;

namespace InGame.Dynamics
{
    public class FakeBrowser : IBrowser
    {
        public void Open() { }
        public void Close() { }
        public void Maximize() { }

        private int getNumber = 0;
        private readonly string[] html;

        public FakeBrowser(string[] html)
        {
            this.html = html;
            Debug.Log("Fake browser has been loaded with " + html.Length + " documents");
        }
        public void GoToUrl(string url) { }
        public void GetDocument(HtmlDocument doc)
        {
            Debug.Log("Get fake document: " + getNumber);
            doc.LoadHtml(html[getNumber]);
            getNumber++;
        }
    }
}