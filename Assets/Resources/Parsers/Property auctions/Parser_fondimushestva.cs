using HtmlAgilityPack;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class Parser_fondimushestva : Parser<PropertyAuctionsLot>
    {
        protected override PropertyAuctionsLot ParseLotOrThrowException(HtmlNode node)
        {
            throw new System.NotImplementedException();
        }
    }
}