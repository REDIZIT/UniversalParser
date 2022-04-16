using System;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.Parse
{
    public class ParseResult<T> : IParseResult where T : Lot
	{
		public List<T> lots = new List<T>();

		public IEnumerable<Lot> EnumerateLots()
        {
            return lots.Cast<Lot>();
        }
		public IEnumerable<Lot> EnumerateUnpackedLots()
        {
            foreach (T lot in lots)
            {
				if (lot is ILotContainer container)
                {
                    foreach (Lot containerLot in container.EnumerateLots())
                    {
                        yield return containerLot;
                    }
                }
                else
                {
                    yield return lot;
                }
            }
        }
		public void AddRange(IEnumerable<Lot> lotsToAdd)
        {
			lots.AddRange(lotsToAdd.Cast<T>());
        }
		public void RemoveWhere(Func<Lot, bool> func)
        {
			lots.RemoveAll(t => func(t));
		}
		public void MergeWith(IParseResult another)
        {
			ParseResult<T> result = (ParseResult<T>)another;
			lots.AddRange(result.lots);
        }
		public void Clear()
        {
			lots.Clear();
        }
		public IParseResult Clone()
        {
			ParseResult<T> clone = new ParseResult<T>();
			clone.lots.AddRange(lots);
			return clone;
        }
	}
}