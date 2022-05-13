using InGame.Parse;

namespace UnityParser
{
    public interface IParser
    {
        ParseProcess process { get; }
        ParseProcess StartParsing(string url, int startPage = 0, int endPage = 0);
        int GetCurrentPageNumberByUrl(string url);
        string GetUrlWithPageNumber(string baseUrl, int pageNumber);
    }
}