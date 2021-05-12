using InGame.Parse;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityParser;

namespace InGame.UI
{
	public class PropertyAuctionsParseUI : MonoBehaviour
	{
		public SelectTableControl selectTableUI;
		public UrlHandlerControl urlControl;
        public SummaryControl summary;

        private Parser<PropertyAuctionsLot> parser;
        private List<ParseResult> results = new List<ParseResult>();

        private void Awake()
        {
            selectTableUI.onTableReset += Clear;
            selectTableUI.onTableSelected += () => urlControl.Refresh(parser, OnParseFinished);
            urlControl.Hide();
        }

        private void Clear()
        {
            urlControl.Clear();
        }

        private void OnParseFinished()
        {
            results.Add(parser.process.result);

            GlobalUI.parseResultWindow.Show(parser.process);
            summary.OnParseFinished(parser.process, Save);
        }

        private void Save()
        {
            if (selectTableUI.workingTableType == SelectTableControl.WorkingTableType.CreateNewTable)
            {
                SaveNewTable(selectTableUI.tableFilePath, results);
            }
            else if (selectTableUI.workingTableType == SelectTableControl.WorkingTableType.ExistingTable)
            {
                SaveToExistingTable(selectTableUI.tableFilePath, results);
            }
        }

        private void SaveNewTable(string filepath, List<ParseResult> results)
        {
            var save = new ParseSave<AvitoLot>(results);
            //ExcelTable table = ExcelSerializer.CreateTable(filepath, save);
            throw new NotImplementedException();
        }

        private void SaveToExistingTable(string filepath, List<ParseResult> results)
        {
            throw new NotImplementedException();
            //Excel excel = ExcelHelper.LoadExcel(filepath);
            //ExcelTable table = excel.Tables[0];

            //var save = new PraseSave<AvitoLot>(results);
            //IEnumerable<string> urls = GetLotsUrls(table);

            //AppendLots(table, save.GetUniqueLots(urls).Cast<AvitoLot>());

            //ExcelHelper.SaveExcel(excel, filepath);
        }

        //private void AppendLots(ExcelTable table, IEnumerable<AvitoLot> lots)
        //      {
        //	int start = table.NumberOfRows;

        //	int row = start;
        //          foreach (AvitoLot lot in lots)
        //          {
        //		row++;
        //		table.SetValue(row, 1, lot.name);
        //		table.SetValue(row, 2, lot.area);
        //		table.SetValue(row, 3, lot.storeys);
        //		table.SetValue(row, 4, lot.price);
        //		table.SetValue(row, 5, lot.hasOnlineView ? "Имеет" : "-");
        //		table.SetValue(row, 6, lot.address);
        //		table.SetValue(row, 7, lot.metro);
        //		table.SetValue(row, 8, lot.agency);
        //		table.SetValue(row, 9, lot.url);
        //          }
        //      }
        //private IEnumerable<string> GetLotsUrls(ExcelTable table)
        //      {
        //          for (int i = 1; i <= table.NumberOfRows; i++)
        //          {
        //		yield return table.GetCell(i, 9).Value;
        //          }
        //      }



    }
}