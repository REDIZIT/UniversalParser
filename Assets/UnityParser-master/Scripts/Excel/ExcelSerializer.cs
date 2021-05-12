using InGame.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityParser
{
	public static class ExcelSerializer
	{ 
		public static ExcelTable CreateTable(string filepath, ParseResult result)
        {
            Excel excel = ExcelHelper.CreateExcel(filepath);
            ExcelTable table = excel.Tables[0];
			

            CreateHeader(table, result.lots[0].GetType());

			

			AppendLots(result.lots[0].GetType(), table, result.lots);

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
			AppendLots(typeof(T), table, lots);

		}
		private static void AppendLots(Type lotType, ExcelTable table, IEnumerable<Lot> lots)
        {
			int row = table.NumberOfRows;

			List<ExcelStringAttribute> attributes = new List<ExcelStringAttribute>();

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

					string value = GetValueByAttribute(attribute, lotType, lot);
					table.SetValue(row, column, value);
				}
			}
		}


        /// <summary>Returns all lots IDs represented in excel table</summary>
        private static IEnumerable<string> GetAllIDs<T>(ExcelTable table) where T : Lot
        {
			int idColumn = 0;
			foreach (FieldInfo field in typeof(T).GetFields())
			{
				if (field.GetCustomAttribute<ExcelStringAttribute>() == null)
					continue;

				idColumn++;

				var at = field.GetCustomAttribute<ExcelIDAttribute>();
				if (at != null) break;
			}


            for (int i = 2; i <= table.NumberOfRows; i++)
            {
				yield return table.GetValue(i, idColumn).ToString();
			}
        }


		private static void CreateHeader(ExcelTable table, Type lotType)
        {
			int column = 0;
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

		private static string GetValueByAttribute(ExcelStringAttribute attribute, Type lotType, Lot lot)
        {
			foreach (var field in lotType.GetFields())
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