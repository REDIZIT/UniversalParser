using HtmlAgilityPack;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
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