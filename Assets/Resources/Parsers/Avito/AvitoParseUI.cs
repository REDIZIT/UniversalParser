using InGame.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
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

		private IParser parser;

		//private List<ParseResult> results = new List<ParseResult>();

		private void Awake()
		{
			selectTableUI.onTableReset += Clear;
			selectTableUI.onTableSelected += OnTableSelected;
			urlControl.Hide();

			parser = CreateParser();
		}

		public virtual IParser CreateParser()
		{
			return new AvitoParser();
		}
		protected virtual IParseSave GetSave(List<ParseResult> results)
        {
			return new ParseSave<AvitoLot>(results);
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
			//results.Add(parser.process.bigResult);

			GlobalUI.parseResultWindow.Show(parser.process);
			summary.OnParseFinished(parser, Save);
		}

		private void Save()
		{
			ParseResult bigResult = summary.GetBigResult();

			if (selectTableUI.workingTableType == SelectTableControl.WorkingTableType.CreateNewTable)
			{
				SaveNewTable(selectTableUI.tableFilePath, bigResult);
			}
            else if (selectTableUI.workingTableType == SelectTableControl.WorkingTableType.ExistingTable)
			{
                SaveToExistingTable(selectTableUI.tableFilePath, bigResult);
            }

			GlobalUI.savedWindow.Show(selectTableUI.tableFilePath);
        }

        private void SaveNewTable(string filepath, ParseResult result)
        {
            ExcelSerializer.CreateTable(filepath, result);
        }

        private void SaveToExistingTable(string filepath, ParseResult result)
        {
			ExcelSerializer.AppendUniqLots(filepath, result);
        }
    }
}