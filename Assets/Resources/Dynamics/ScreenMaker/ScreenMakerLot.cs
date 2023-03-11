using UnityParser;

namespace InGame.Dynamics
{
    public class ScreenMakerLot : Lot
    {
        [ExcelString("Метро")]
        public string metro;

        [ExcelString("Адрес")]
        public string address;

        [ExcelString("Дом")]
        public string levels;

        [ExcelString("Цена")]
        public string price;

        [ExcelString("Описание")]
        public string description;

        [ExcelString("Балкон")]
        public string balcony;

        [ExcelString("Окна")]
        public string outsideView;

        [ExcelString("Санузел")]
        public string bathroom;

        [ExcelString("Лифт")]
        public string lift;

        [ExcelString("Дата")]
        public string date;

        [ExcelString("Ссылка на объявление")]
        public string lotUrl;
    }
}