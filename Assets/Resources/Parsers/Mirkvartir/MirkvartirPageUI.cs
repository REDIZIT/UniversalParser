using InGame.Parse;
using System.Collections.Generic;
using UnityParser;

namespace InGame.UI
{
    public class MirkvartirPageUI : AvitoParseUI
	{
        public override IParser CreateParser()
        {
			return new MirkvartirParser();
		}

        protected override IParseSave GetSave(List<ParseResult> results)
        {
            return new ParseSave<MirkvartirLot>(results);
        }
    }
}