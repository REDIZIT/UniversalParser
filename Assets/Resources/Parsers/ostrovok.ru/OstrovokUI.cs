using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.UI
{
    public class OstrovokUI : AvitoParseUI
    {
        public override IParser CreateParser()
        {
            OstrovokParser parser = new OstrovokParser();
            parser.Setup(selectTableUI);
            return parser;
        }

        protected override IParseSave GetSave(List<IParseResult> results)
        {
            return new ParseSave<OstrovokLot>(results.Cast<ParseResult<OstrovokLot>>().ToList());
        }
    }
}