using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using UnityParser;

namespace InGame.Parse
{
    public class CIANParser : Parser<CIANLot>
    {
        protected override string UrlPageArgument => "page";

        protected override IEnumerable<HtmlNode> GetNodesToParse(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        protected override CIANLot ParseLotOrThrowException(HtmlNode node)
        {
            throw new NotImplementedException();
        }
    }
    public class CIANLot : Lot
    {

    }
}