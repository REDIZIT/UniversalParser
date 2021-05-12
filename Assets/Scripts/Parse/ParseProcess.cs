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

    public interface IParseSave
    {
        IEnumerable<Lot> GetAllLots();
        IEnumerable<Lot> GetUniqueLots(IEnumerable<string> urls);
		Type GetLotType();
    }

	public class ParseSave<T> : IParseSave where T : Lot
	{
		public List<ParseResult> results;

        public ParseSave(List<ParseResult> results)
        {
			this.results = results;
        }
		public IEnumerable<Lot> GetAllLots()
        {
			return results.SelectMany(r => r.lots).DistinctBy(l => l.url).Cast<T>();
		}
		public IEnumerable<Lot> GetUniqueLots(IEnumerable<string> urls)
        {
			return GetAllLots().Where(l => urls.Contains(l.url) == false);
        }

		public Type GetLotType()
        {
			return typeof(T);
        }
    }

	public class ParseResult
	{
		public List<Lot> lots = new List<Lot>();
	}
}