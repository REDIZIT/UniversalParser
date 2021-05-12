using InGame.Parse;
using System.Collections.Generic;
using UnityParser;

namespace InGame.UI
{
    public class ParkingParseUI : AvitoParseUI
	{
        public override IParser CreateParser()
        {
			return new MirkvartirParser();
		}

        protected override IParseSave GetSave(List<ParseResult> results)
        {
            return new ParseSave<MirkvatrirLot>(results);
        }
    }
}