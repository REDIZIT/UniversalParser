using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class ParseProcess
    {
		public IParseResult bigResult;
		public List<IParseResult> results = new List<IParseResult>();
		public IParseResult currentPageResult;

		public State state;
		public float progress;
		public string progressMessage;

		public List<string> urlsToParse;

		public Exception exception;

		public Action onPageParsed;
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
		public List<ParseResult<T>> results;

        public ParseSave(List<ParseResult<T>> results)
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

	public interface IParseResult
    {
		IEnumerable<Lot> EnumerateLots();
		IEnumerable<Lot> EnumerateUnpackedLots();
		void MergeWith(IParseResult another);
		void AddRange(IEnumerable<Lot> lots);
		void Clear();
		void RemoveWhere(Func<Lot, bool> func);
		IParseResult Clone();
    }
}