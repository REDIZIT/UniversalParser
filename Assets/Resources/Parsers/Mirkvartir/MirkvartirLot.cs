using UnityParser;

namespace InGame.Parse
{
    public class MirkvartirLot : Lot
    {
		[ExcelString("Комнаты")]
		public string rooms;

		[ExcelString("Площадь")]
		public string area;

		[ExcelString("Этаж")]
		public string floor;

		[ExcelString("Адрес")]
		public string address;

		[ExcelString("Метро/Район")]
        public string metroOrDistrict;

		[ExcelString("Цена")]
		public string price;

        public MirkvartirLot()
        {

        }
		public MirkvartirLot(string url) : base(url)
        {

        }
    }
}