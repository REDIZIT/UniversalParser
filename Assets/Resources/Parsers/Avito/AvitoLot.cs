using System;
using UnityParser;

namespace InGame.Parse
{
    public class AvitoLot : Lot
	{
		[ExcelString("Название")]
		public string name;

		[ExcelString("Площадь")]
		public string area;

		[ExcelString("Этажность")]
		public string storeys;

		[ExcelString("Цена")]
		public string price;

		[ExcelBool("Онлайн просмотр", "-", "Имеет")]
		public bool hasOnlineView;

		[ExcelString("Адрес")]
		public string address;

		[ExcelString("Метро")]
		public string metro;

		[ExcelString("Агенство")]
		public string agency;
	}
}