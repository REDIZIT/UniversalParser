using SFB;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    /// <summary>Control for selecting excel table for next working</summary>
	public class SelectTableControl : MonoBehaviour
	{
        public WorkingTableType workingTableType;
        public string tableFilePath;

        public Action onTableReset;
        public Action onTableSelected;

        [SerializeField] private GameObject selectGroup, deselectGroup;
        [SerializeField] private Text tableNameText;

        

        public enum WorkingTableType
        {
            NotSelected,
            CreateNewTable,
            ExistingTable
        }

        



        private void Update()
        {
            selectGroup.SetActive(workingTableType == WorkingTableType.NotSelected);
            deselectGroup.SetActive(workingTableType != WorkingTableType.NotSelected);

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

                workingTableType = WorkingTableType.CreateNewTable;
                tableFilePath = filepath;

                onTableReset?.Invoke();
                onTableSelected?.Invoke();
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

                workingTableType = WorkingTableType.ExistingTable;
                tableFilePath = filepath;

                onTableReset?.Invoke();
                onTableSelected?.Invoke();
            });
        }
        public void ClickChangeTable()
        {
            workingTableType = WorkingTableType.NotSelected;
            tableFilePath = "";

            onTableReset?.Invoke();
        }
    }
}