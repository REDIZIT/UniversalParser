using System;
using UnityParser;

namespace InGame.Parse
{
    public class AvitoLot : Lot
	{
		[ExcelString("��������")]
		public string name;

		[ExcelString("�������")]
		public string area;

		[ExcelString("���������")]
		public string storeys;

		[ExcelString("����")]
		public string price;

		[ExcelBool("������ ��������", "-", "�����")]
		public bool hasOnlineView;

		[ExcelString("�����")]
		public string address;

		[ExcelString("�����")]
		public string metro;

		[ExcelString("��������")]
		public string agency;
	}
}