using System;
using System.Collections.Generic;
using System.Linq;

namespace InGame.Parse
{
    public class ParseProcess
    {
		public ParseResult result;

		public State state;
		public float progress;
		public string progressMessage;

		public Exception exception;

		public Action onfinished;

		public enum State
        {
			Running, Finished
        }
	}

	public class PraseSave
    {
		public List<ParseResult> results;

        public PraseSave(List<ParseResult> results)
        {
			this.results = results;
        }
		public IEnumerable<AvitoLot> GetAllLots()
        {
			return results.SelectMany(r => r.lots);

		}
		public IEnumerable<AvitoLot> GetUniqueLots(IEnumerable<string> urls)
        {
			return GetAllLots().Where(l => urls.Contains(l.url) == false);
        }
    }

	public class ParseResult
	{
		public List<AvitoLot> lots = new List<AvitoLot>();
	}
}