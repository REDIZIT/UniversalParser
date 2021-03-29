using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityParser
{
	public static class ExcelSerializer
	{ 
		public static ExcelTable CreateTable<T>(string filepath, IEnumerable<T> lots) where T : Lot
        {
            Excel excel = ExcelHelper.CreateExcel(filepath);
            ExcelTable table = excel.Tables[0];
			

            CreateHeader<T>(table);

			AppendLots(table, lots);

			//ApplyWidth<T>(table);

			ExcelHelper.SaveExcel(excel, filepath);

			return table;            
        }
        /// <summary>
		/// Append to table only these lots, which are not presented in table yet. Comparing by <see cref="ExcelIDAttribute"/>
		/// </summary>
        public static void AppendUniqLots<T>(string filepath, IEnumerable<T> lots) where T : Lot
        {
			Excel excel = ExcelHelper.LoadExcel(filepath);
			ExcelTable table = excel.Tables[0];

			IEnumerable<string> tableIDs = GetAllIDs<T>(table);
			IEnumerable<T> lotsToAppend = lots.Where(l => tableIDs.Contains(GetIDValue(l)) == false);

			AppendLots(table, lotsToAppend);

			ExcelHelper.SaveExcel(excel, filepath);
		}

		private static void AppendLots<T>(ExcelTable table, IEnumerable<T> lots) where T : Lot
        {
			int row = table.NumberOfRows;

			List<ExcelStringAttribute> attributes = new List<ExcelStringAttribute>();

			Type lotType = typeof(T);
			foreach (FieldInfo field in lotType.GetFields())
			{
				var attribute = field.GetCustomAttribute<ExcelStringAttribute>();
				if (attribute != null)
				{
					attributes.Add(attribute);
				}
			}


			foreach (var lot in lots)
            {
				row++;

				int column = 0;
                foreach (ExcelStringAttribute attribute in attributes)
                {
					column++;

					string value = GetValueByAttribute(attribute, lot);
					table.SetValue(row, column, value);
				}
			}
        }
		//private static void ApplyWidth<T>(ExcelTable table)
  //      {
		//	int column = 0;
		//	foreach (var attribute in typeof(T).GetFields().Select(f => f.GetCustomAttribute<ExcelStringAttribute>()))
  //          {
		//		if (attribute != null)
  //              {
		//			column++;
		//			float width = attribute.width;

		//			table.SetColumnWidth(column, width);
  //              }
  //          }
  //      }



		private static IEnumerable<string> GetAllIDs<T>(ExcelTable table) where T : Lot
        {
			int idColumn = 0;
			foreach (FieldInfo field in typeof(T).GetFields())
			{
				idColumn++;

				var at = field.GetCustomAttribute<ExcelIDAttribute>();
				if (at != null) break;
			}


            for (int i = 2; i < table.NumberOfRows; i++)
            {
				yield return table.GetValue(i, idColumn).ToString();
			}
        }


		private static void CreateHeader<T>(ExcelTable table)
        {
			int column = 0;
			Type lotType = typeof(T);
            foreach (FieldInfo field in lotType.GetFields())
            {
				var attribute = field.GetCustomAttribute<ExcelStringAttribute>();
				if (attribute != null)
                {
					column++;
					table.SetValue(1, column, attribute.name);
				}
			}
        }

		private static string GetValueByAttribute<T>(ExcelStringAttribute attribute, T lot) where T : Lot
        {
			foreach (var field in typeof(T).GetFields())
			{
				var at = field.GetCustomAttribute<ExcelStringAttribute>();
				if (at != null && at.name == attribute.name)
				{
					object fieldValue = field.GetValue(lot);
					if (fieldValue == null) return "";
					return attribute.ToString(fieldValue);
				}
			}

			throw new Exception($"Can't get value for ExcelPropertyAttribute with name '{attribute.name}'");
		}
		private static string GetIDValue<T>(T lot) where T : Lot
		{
			foreach (var field in typeof(T).GetFields())
			{
				var at = field.GetCustomAttribute<ExcelIDAttribute>();
				if (at != null)
				{
					return field.GetValue(lot).ToString();
				}
			}

			throw new Exception($"Can't get id for lot");
		}
	}
}