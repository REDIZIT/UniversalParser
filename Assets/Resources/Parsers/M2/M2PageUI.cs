using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.UI
{
    public class M2PageUI : AvitoParseUI
    {
        public override IParser CreateParser()
        {
            return new M2Parser();
        }

        protected override IParseSave GetSave(List<IParseResult> results)
        {
            return new ParseSave<MirkvartirLot>(results.Cast<ParseResult<MirkvartirLot>>().ToList());
        }
    }
}