using InGame.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UnityParser
{
	public static class ExcelSerializer
	{ 
		public static ExcelTable CreateTable(string filepath, IParseResult result)
        {
            Excel excel = ExcelHelper.CreateExcel(filepath);
            ExcelTable table = excel.Tables[0];

            CreateHeader(table, result.EnumerateUnpackedLots().First().GetType());

			AppendLots(result.EnumerateUnpackedLots().First().GetType(), table, result.EnumerateUnpackedLots());

			ExcelHelper.SaveExcel(excel, filepath);

			return table;
        }

        /// <summary>
		/// Append to table only these lots, which are not presented in table yet. Comparing by <see cref="ExcelIDAttribute"/>
		/// </summary>
        public static void AppendUniqLots(string filepath, IParseResult result)
        {
			Excel excel = ExcelHelper.LoadExcel(filepath);
			ExcelTable table = excel.Tables[0];

			IEnumerable<string> tableIDs = GetAllIDs(result.EnumerateLots().First().GetType(), table);
			IEnumerable<Lot> lotsToAppend = result.EnumerateLots().Where(l => tableIDs.Contains(GetIDValue(l)) == false);

			AppendLots(result.EnumerateLots().First().GetType(), table, lotsToAppend);

			ExcelHelper.SaveExcel(excel, filepath);
		}

		public static IEnumerable<string> LoadIDs<T>(string filepath) where T : Lot
        {
			Excel excel = ExcelHelper.LoadExcel(filepath);
			return GetAllIDs(typeof(T), excel.Tables[0]);
        }
		public static IEnumerable<T> LoadLots<T>(string filepath) where T : Lot
		{
            Excel excel = ExcelHelper.LoadExcel(filepath);


			// Extract ExcelStringAttributes from Lot class
			List<ExcelStringAttribute> stringAttributes = new();
			Dictionary<ExcelStringAttribute, FieldInfo> fieldInfoByAttribute = new();

            foreach (FieldInfo field in typeof(T).GetFields())
            {
				ExcelStringAttribute attribute = field.GetCustomAttribute<ExcelStringAttribute>();
				if (attribute != null)
				{
                    stringAttributes.Add(attribute);
                    fieldInfoByAttribute.Add(attribute, field);
                }
            }


			// Extract Excel table header columns
            int headerWidth = excel.Tables[0].NumberOfColumns;
			List<string> headerColumns = new List<string>();

			for (int i = 1; i <= headerWidth; i++)
			{
				headerColumns.Add((string)excel.Tables[0].GetValue(1, i));
			}


			// Match ExcelStringAttrinute with it's order in Excel table's header
            Dictionary<int, ExcelStringAttribute> lotFieldByHeaderIndex = new();

			for (int i = 0; i < headerColumns.Count; i++)
			{
				string headerColumn = headerColumns[i];
				ExcelStringAttribute matchedAttribute = stringAttributes.FirstOrDefault(a => a.name == headerColumn);

				if (matchedAttribute == null)
				{
					Debug.Log("No matching attribute found for: " + headerColumn);
				}
				else
				{
					lotFieldByHeaderIndex.Add(i + 1, matchedAttribute);
				}
			}


			// Enumerate Excel table's row by dictionary
			for (int i = 2; i <= excel.Tables[0].NumberOfRows; i++)
			{
				T lot = Activator.CreateInstance<T>();

				foreach (KeyValuePair<int, ExcelStringAttribute> kv in lotFieldByHeaderIndex)
				{
					int column = kv.Key;
					ExcelStringAttribute attribute = kv.Value;

					string cellValue = (string)excel.Tables[0].GetValue(i, column);

					fieldInfoByAttribute[attribute].SetValue(lot, cellValue);
				}

				yield return lot;
			}
        }

		private static void AppendLots(Type lotType, ExcelTable table, IEnumerable<Lot> lots)
        {
			int row = table.NumberOfRows;

			List<ExcelStringAttribute> attributes = new List<ExcelStringAttribute>();
            Debug.Log($"{lots.Count()} lots <{lotType}> were saved");
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
        private static IEnumerable<string> GetAllIDs(Type lotType, ExcelTable table)
        {
			int idColumn = 0;
			foreach (FieldInfo field in lotType.GetFields())
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