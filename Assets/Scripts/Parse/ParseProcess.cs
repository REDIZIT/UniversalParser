using System;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

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

	public class PraseSave<T> where T : Lot
    {
		public List<ParseResult> results;

        public PraseSave(List<ParseResult> results)
        {
			this.results = results;
        }
		public IEnumerable<T> GetAllLots()
        {
			return results.SelectMany(r => r.lots).Cast<T>();

		}
		public IEnumerable<T> GetUniqueLots(IEnumerable<string> urls)
        {
			return GetAllLots().Where(l => urls.Contains(l.url) == false);
        }
    }

	public class ParseResult
	{
		public List<Lot> lots = new List<Lot>();
	}
}