using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityParser;

namespace InGame.UI
{
    public class SummaryUI : MonoBehaviour
	{
		public SelectTableUI selectTableUI;
		public CloseExcelWindow closeExcelWindow;
		public NotSafeExitWindow notSafeExitWindow;

		[SerializeField] private Transform content;
		[SerializeField] private SummaryUIItem itemPrefab;
		[SerializeField] private GameObject separatorPrefab;

		[SerializeField] private Text pagesCountText, newPositionsCountText, errorsCountText;

		private List<ParseResult> results = new List<ParseResult>();
		private bool isSaved;
		private bool forceQuit;



        private void Awake()
        {
			Application.wantsToQuit += () =>
			{
				Debug.Log("Wants to quit");

				if (forceQuit) return true;
				//if (forceQuit || isSaved) return true;

				notSafeExitWindow.Show(new NotSafeExitWindow.Arguments()
				{
					 onExitWithoutSave = () => { forceQuit = true; Application.Quit(); },
					 onSaveAndExit = () => { ClickSave(); forceQuit = true; Application.Quit(); }
				});

				return false;
			};
		}
        public void OnParseFinished(ParseProcess process)
        {
			if (process.result == null) return;
			results.Add(process.result);

			isSaved = false;

			RefreshContent();
		}
		public void ClearResults()
        {
			results.Clear();
			RefreshContent();
		}

		public void ClickSave()
        {
			isSaved = false;

			try
            {
				if (selectTableUI.workingTableType == SelectTableUI.WorkingTableType.CreateNewTable)
				{
					SaveNewTable(selectTableUI.tableFilePath, results);

					////// Change mode to existing table because next time user waits append result to created table
					////// If not change mode, table will be recreated every time user click save in this session
					////selectTableUI.workingTableType = SelectTableUI.WorkingTableType.ExistingTable;
				}
				else if (selectTableUI.workingTableType == SelectTableUI.WorkingTableType.ExistingTable)
				{
					SaveToExistingTable(selectTableUI.tableFilePath, results);
				}
			}
            catch(Exception err)
            {
				closeExcelWindow.Show(err);
				Debug.LogError(err);
			}

			isSaved = true;
		}


		private void SaveNewTable(string filepath, List<ParseResult> results)
        {
			PraseSave save = new PraseSave(results);
			ExcelTable table = ExcelSerializer.CreateTable(filepath, save.GetAllLots());
		}
		private void SaveToExistingTable(string filepath, List<ParseResult> results)
        {
			Excel excel = ExcelHelper.LoadExcel(filepath);
			ExcelTable table = excel.Tables[0];

			PraseSave save = new PraseSave(results);
			IEnumerable<string> urls = GetLotsUrls(table);

			AppendLots(table, save.GetUniqueLots(urls));

			ExcelHelper.SaveExcel(excel, filepath);
		}

		private void AppendLots(ExcelTable table, IEnumerable<AvitoLot> lots)
        {
			int start = table.NumberOfRows;

			int row = start;
            foreach (AvitoLot lot in lots)
            {
				row++;
				table.SetValue(row, 1, lot.name);
				table.SetValue(row, 2, lot.area);
				table.SetValue(row, 3, lot.storeys);
				table.SetValue(row, 4, lot.price);
				table.SetValue(row, 5, lot.hasOnlineView ? "Имеет" : "-");
				table.SetValue(row, 6, lot.address);
				table.SetValue(row, 7, lot.metro);
				table.SetValue(row, 8, lot.agency);
				table.SetValue(row, 9, lot.url);
            }
        }
		private IEnumerable<string> GetLotsUrls(ExcelTable table)
        {
            for (int i = 1; i <= table.NumberOfRows; i++)
            {
				yield return table.GetCell(i, 9).Value;
            }
        }




		private void RefreshContent()
		{
			ClearChildren(content);

			int i = 0;
			foreach (ParseResult result in results)
			{
				GameObject inst = Instantiate(itemPrefab.gameObject, content);
				SummaryUIItem uii = inst.GetComponent<SummaryUIItem>();
				uii.Refresh(result, i);

				i++;
			}

			pagesCountText.text = "Страниц обработано: " + i;
			newPositionsCountText.text = "Новых позиций: " + results.Sum(r => r.lots.Count(l => l.exception == null));
			errorsCountText.text = "Ошибок: " + results.Sum(r => r.lots.Count(l => l.exception != null));
		}
		private void ClearChildren(Transform content)
        {
            foreach (Transform child in content)
            {
				Destroy(child.gameObject);
            }
		}
	}
}