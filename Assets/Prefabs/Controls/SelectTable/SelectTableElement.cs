using InGame.Parse;
using SFB;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityParser;

namespace InGame.Dynamics
{

    public class SelectTableElement : DynamicElement<ISelectTable.Model>, ISelectTable
    {
        public bool IsSelected => string.IsNullOrWhiteSpace(tableFilePath) == false;

        public WorkingTableType workingTableType;
        public TableSelectMode selectMode;
        public string tableFilePath;

        public Action onTableReset;
        public Action onTableSelected;

        [SerializeField] private GameObject selectGroup, deselectGroup;
        [SerializeField] private Text tableNameText;

        [SerializeField] private GameObject newTableButton, oldTableButton, separator;

        public enum TableSelectMode
        {
            NewAndOld,
            OnlyOld,
            OnlyNew
        }
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

            newTableButton.SetActive(selectMode == TableSelectMode.NewAndOld || selectMode == TableSelectMode.OnlyNew);
            oldTableButton.SetActive(selectMode == TableSelectMode.NewAndOld || selectMode == TableSelectMode.OnlyOld);
            separator.SetActive(selectMode == TableSelectMode.NewAndOld);


            if (workingTableType != WorkingTableType.NotSelected)
            {
                tableNameText.text = Path.GetFileName(tableFilePath);
            }

            IsValid = IsSelected;
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

            StandaloneFileBrowser.OpenFilePanelAsync("Выбрать таблицу", "", "xlsx", false, (filepathes) =>
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
        public void SaveResult(IParseResult result)
        {
            if (workingTableType == WorkingTableType.CreateNewTable)
            {
                ExcelSerializer.CreateTable(tableFilePath, result);
            }
            else
            {
                ExcelSerializer.AppendUniqLots(tableFilePath, result);
            }
        }
        public IEnumerable<string> Load<T>() where T : Lot
        {
            if (workingTableType == WorkingTableType.CreateNewTable)
            {
                return null;
            }
            else
            {
                return ExcelSerializer.LoadIDs<T>(tableFilePath);
            }
        }
    }
}