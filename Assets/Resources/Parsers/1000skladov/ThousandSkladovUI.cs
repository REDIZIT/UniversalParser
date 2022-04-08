using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.UI
{
    public class ThousandSkladovUI : AvitoParseUI
    {
        public override IParser CreateParser()
        {
            return new ThousandSkladovParser();
        }

        protected override IParseSave GetSave(List<IParseResult> results)
        {
            return new ParseSave<StorageLot>(results.Cast<ParseResult<StorageLot>>().ToList());
        }
    }
}