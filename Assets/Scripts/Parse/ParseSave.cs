using System;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.Parse
{
    public class ParseSave<T> : IParseSave where T : Lot
	{
		public List<ParseResult<T>> results;

        public ParseSave()
        {
            results = new List<ParseResult<T>>();
        }
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
}