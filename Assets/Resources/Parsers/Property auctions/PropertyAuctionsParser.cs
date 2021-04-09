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
        [ExcelString("Номер лота")]
        public string lotNumber;

        [ExcelString("Кадастровые номера")]
        public string cadastralNumbers;

        [ExcelString("Адрес")]
        public string address;
    }
}