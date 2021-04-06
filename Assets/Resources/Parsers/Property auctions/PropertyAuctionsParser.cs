using HtmlAgilityPack;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class PropertyAuctionsParser : Parser<PropertyAuctionsLot>
    {
        protected override PropertyAuctionsLot ParseLotOrThrowException(HtmlNode node)
        {
            throw new System.NotImplementedException();
        }
    }

    public class PropertyAuctionsLot : Lot
    {

    }
}