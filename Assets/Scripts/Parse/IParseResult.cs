using System;
using System.Collections.Generic;
using UnityParser;

namespace InGame.Parse
{
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