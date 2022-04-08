using InGame.Parse;
using System.Collections.Generic;
using UnityParser;

namespace InGame.UI
{
    public class ThousandSkladovUI : AvitoParseUI
    {
        public override IParser CreateParser()
        {
            return new ThousandSkladovParser();
        }

        protected override IParseSave GetSave(List<ParseResult> results)
        {
            return new ParseSave<StorageLot>(results);
        }
    }
}