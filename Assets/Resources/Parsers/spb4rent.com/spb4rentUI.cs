using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.UI
{
    public class spb4rentUI : AvitoParseUI
    {
        public override IParser CreateParser()
        {
            return new Spb4rentParser();
        }

        protected override IParseSave GetSave(List<IParseResult> results)
        {
            return new ParseSave<StorageLot>(results.Cast<ParseResult<StorageLot>>().ToList());
        }
    }
}