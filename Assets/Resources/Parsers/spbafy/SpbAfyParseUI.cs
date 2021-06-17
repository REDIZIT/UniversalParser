using InGame.Parse;
using System.Collections.Generic;
using UnityParser;

namespace InGame.UI
{
    public class SpbAfyParseUI : AvitoParseUI
    {
        public override IParser CreateParser()
        {
            return new SpbAfyParser();
        }

        protected override IParseSave GetSave(List<ParseResult> results)
        {
            return new ParseSave<MirkvartirLot>(results);
        }
    }
}