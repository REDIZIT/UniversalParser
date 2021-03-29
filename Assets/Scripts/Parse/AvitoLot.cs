using System;
using UnityParser;

namespace InGame.Parse
{
    public class AvitoLot : Lot
	{
		[ExcelString("���������� ������", 100)]
		public string name;

		[ExcelString("�������", 40)]
		public string area;

		[ExcelString("���������", 40)]
		public string storeys;

		[ExcelString("����", 60)]
		public string price;

		[ExcelBool("������ ��������", 40, "-", "�����")]
		public bool hasOnlineView;

		[ExcelString("�����", 400)]
		public string address;

		[ExcelString("�����", 300)]
		public string metro;

		[ExcelString("��������", 300)]
		public string agency;

		[ExcelID]
		[ExcelString("������", 1200)]
		public string url;
	}
}