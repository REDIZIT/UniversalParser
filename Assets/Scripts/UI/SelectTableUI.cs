using InGame.Settings;
using SFB;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
	public class SelectTableUI : MonoBehaviour
	{
        public ParseUI parseUI;

        [SerializeField] private GameObject selectFirstGroup, changeGroup;
        [SerializeField] private Text tableNameText;

        public WorkingTableType workingTableType;
        public string tableFilePath;

        public enum WorkingTableType
        {
            NotSelected,
            CreateNewTable,
            ExistingTable
        }

        



        private void Update()
        {
            selectFirstGroup.SetActive(workingTableType == WorkingTableType.NotSelected);
            changeGroup.SetActive(workingTableType != WorkingTableType.NotSelected);

            if (workingTableType != WorkingTableType.NotSelected)
            {
                tableNameText.text = Path.GetFileName(tableFilePath);
            }
        }

        public void ClickUseNewTable()
        {
            workingTableType = WorkingTableType.NotSelected;

            StandaloneFileBrowser.SaveFilePanelAsync("Создать новую таблицу", "", "Таблица.xlsx", "xlsx", (filepath) =>
            {
                if (string.IsNullOrEmpty(filepath)) return;

                //SettingsManager.settings.FilebrowserLastUsedDirectory = filepath;
                //SettingsManager.Save();

                workingTableType = WorkingTableType.CreateNewTable;
                tableFilePath = filepath;

                parseUI.Clear();
            });
        }
        public void ClickUseExistingTable()
        {
            workingTableType = WorkingTableType.NotSelected;

            StandaloneFileBrowser.OpenFilePanelAsync("Выберите таблицу", "", "xlsx", false, (filepathes) =>
            {
                if (filepathes == null || filepathes.Length == 0) return;
                string filepath = filepathes[0];

                if (string.IsNullOrEmpty(filepath)) return;

                //SettingsManager.settings.FilebrowserLastUsedDirectory = filepath;
                //SettingsManager.Save();

                workingTableType = WorkingTableType.ExistingTable;
                tableFilePath = filepath;

                parseUI.Clear();
            });
        }
        public void ClickChangeTable()
        {
            workingTableType = WorkingTableType.NotSelected;
            tableFilePath = "";

            parseUI.Clear();
        }
    }
}