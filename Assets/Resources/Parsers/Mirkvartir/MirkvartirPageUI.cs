using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.UI
{
    public class MirkvartirPageUI : AvitoParseUI
	{
        public override IParser CreateParser()
        {
			return new MirkvartirParser();
		}

        protected override IParseSave GetSave(List<IParseResult> results)
        {
            return new ParseSave<MirkvartirLot>(results.Cast<ParseResult<MirkvartirLot>>().ToList());
        }
    }
}