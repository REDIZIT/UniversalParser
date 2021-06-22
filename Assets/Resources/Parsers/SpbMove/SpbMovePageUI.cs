using InGame.Parse;
using InGame.UI;
using System.Collections.Generic;
using UnityParser;

namespace Assets.Resources.Parsers.SpbMove
{
	public class SpbMovePageUI : AvitoParseUI
	{
        public override IParser CreateParser()
        {
            return new SpbMoveParser();
        }

        protected override IParseSave GetSave(List<ParseResult> results)
        {
            return new ParseSave<MirkvartirLot>(results);
        }
    }
}