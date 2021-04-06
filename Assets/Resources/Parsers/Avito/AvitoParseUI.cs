using InGame.Parse;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityParser;

namespace InGame.UI
{
	public class AvitoParseUI : MonoBehaviour
	{
		public SelectTableControl selectTableUI;
		public UrlHandlerControl urlControl;
		public SummaryControl summary;

		private AvitoParser parser;

		private List<ParseResult> results = new List<ParseResult>();

		private void Awake()
        {
			selectTableUI.onTableReset += Clear;
			selectTableUI.onTableSelected += OnTableSelected;
			urlControl.Hide();

			parser = new AvitoParser();
		}

		public void OnTableSelected()
        {
			urlControl.Refresh(parser, OnParseFinished);
		}
		public void Clear()
        {
			urlControl.Clear();
			summary.ClearResults();
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
            ExcelSerializer.CreateTable(filepath, save.GetAllLots());
        }

        private void SaveToExistingTable(string filepath, List<ParseResult> results)
        {
            var save = new ParseSave<AvitoLot>(results);
            ExcelSerializer.AppendUniqLots(filepath, save.GetAllLots());
        }
    }
}