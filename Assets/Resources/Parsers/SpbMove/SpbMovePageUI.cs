using InGame.Parse;
using InGame.UI;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace Assets.Resources.Parsers.SpbMove
{
	public class SpbMovePageUI : AvitoParseUI
	{
        public override IParser CreateParser()
        {
            return new SpbMoveParser();
        }

        protected override IParseSave GetSave(List<IParseResult> results)
        {
            return new ParseSave<MirkvartirLot>(results.Cast<ParseResult<MirkvartirLot>>().ToList());
        }
    }
}