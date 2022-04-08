using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityParser;

namespace InGame.UI
{
	public class AvitoParseUI : MonoBehaviour
	{
		public SelectTableControl selectTableUI => page.selectTableUI;
		public UrlHandlerControl urlControl => page.urlControl;
		public SummaryControl summary => page.summary;

		private IParser parser;
		private ParserPage page;


		private void Start()
		{
			page = GetComponent<ParserPage>();

			selectTableUI.onTableReset += Clear;
			selectTableUI.onTableSelected += OnTableSelected;
			urlControl.Hide();

			parser = CreateParser();
		}

		public virtual IParser CreateParser()
		{
			return new AvitoParser();
		}
		protected virtual IParseSave GetSave(List<IParseResult> results)
        {
			return new ParseSave<AvitoLot>(results.Cast<ParseResult<AvitoLot>>().ToList());
		}


		public void OnTableSelected()
        {
			urlControl.Refresh(parser, OnParseFinished, OnPageParsed);
		}
		public void Clear()
        {
			urlControl.Clear();
			summary.ClearResults();
		}


		private void OnPageParsed()
        {
			summary.OnPageParsed(parser, Save);
			Save(false);
		}
		private void OnParseFinished()
        {
			GlobalUI.parseResultWindow.Show(parser.process);
		}

		private void Save()
        {
			Save(true);
        }
		private void Save(bool showWindow)
		{
			IParseResult bigResult = summary.GetBigResult();

			if (selectTableUI.workingTableType == SelectTableControl.WorkingTableType.CreateNewTable)
			{
				SaveNewTable(selectTableUI.tableFilePath, bigResult);
			}
            else if (selectTableUI.workingTableType == SelectTableControl.WorkingTableType.ExistingTable)
			{
                SaveToExistingTable(selectTableUI.tableFilePath, bigResult);
            }

			if (showWindow)
            {
				GlobalUI.savedWindow.Show(selectTableUI.tableFilePath);
			}
        }

        private void SaveNewTable(string filepath, IParseResult result)
        {
            ExcelSerializer.CreateTable(filepath, result);
        }

        private void SaveToExistingTable(string filepath, IParseResult result)
        {
			ExcelSerializer.AppendUniqLots(filepath, result);
        }
    }
}