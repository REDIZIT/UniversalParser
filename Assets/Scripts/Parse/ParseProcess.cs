using System;
using System.Collections.Generic;
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
}