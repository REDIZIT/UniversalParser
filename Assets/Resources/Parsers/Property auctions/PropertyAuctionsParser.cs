using HtmlAgilityPack;
using UnityEngine;
using UnityParser;

namespace InGame.Parse
{
    public class PropertyAuctionsLot : Lot
    {
        [ExcelString("����� ����")]
        public string lotNumber;

        [ExcelString("����������� ������")]
        public string cadastralNumbers;

        [ExcelString("�����")]
        public string address;
    }
}