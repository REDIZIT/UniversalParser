using System;
using UnityParser;

namespace InGame.Parse
{
    public class AvitoLot : Lot
	{
		[ExcelString("Количество комнат", 100)]
		public string name;

		[ExcelString("Площадь", 40)]
		public string area;

		[ExcelString("Этажность", 40)]
		public string storeys;

		[ExcelString("Цена", 60)]
		public string price;

		[ExcelBool("Онлайн просмотр", 40, "-", "Имеет")]
		public bool hasOnlineView;

		[ExcelString("Адрес", 400)]
		public string address;

		[ExcelString("Метро", 300)]
		public string metro;

		[ExcelString("Агенство", 300)]
		public string agency;

		[ExcelID]
		[ExcelString("Ссылка", 1200)]
		public string url;
	}
}