using InGame.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityParser;

namespace InGame.UI
{
    public class CIANUI : AvitoParseUI
    {
        public override IParser CreateParser()
        {
            CIANParser parser = new CIANParser();
            //parser.Setup(selectTableUI);
            return parser;
        }

        protected override IParseSave GetSave(List<IParseResult> results)
        {
            //return new ParseSave<OstrovokLot>(results.Cast<ParseResult<OstrovokLot>>().ToList());
            throw new NotImplementedException();
        }
    }
}