using HtmlAgilityPack;

namespace InGame.Dynamics
{
    public interface IBrowser
    {
        void Open();
        void Close();
        void GoToUrl(string url);
        void GetDocument(HtmlDocument documentToUpdate);
    }
}