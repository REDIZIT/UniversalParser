using Newtonsoft.Json;
using RestSharp.Contrib;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Bridge;
using UnityEngine;
using UnityParser;
using Zenject;
using Debug = UnityEngine.Debug;

namespace InGame.Dynamics
{
    public class ScreenMakerDynamic : DynamicParser
    {
        private IInputField folderSelect;
        private ISelectTable tableSelect;

        [Inject]
        private void Construct(ISelectTable tableSelect, IInputField folderSelect)
        {
            this.tableSelect = tableSelect;
            this.folderSelect = folderSelect;

            tableSelect.Setup(new ISelectTable.Model()
            {
                mode = SelectTableElement.TableSelectMode.OnlyOld
            });
            folderSelect.Setup(new IInputField.Model()
            {
                labelText = "Куда сохранять pdf файлы",
                placeholderText = "Путь до папки",
                validityCheckFunc = (s) => Directory.Exists(s)
            });

            BakeElements();
        }

        protected override void OnStart()
        {
            string templatePath = Application.streamingAssetsPath + "/Bridge/template.docx";

            ScreenMakerLot[] lots = GetLots();

            List<Process> processes = new List<Process>();

            int groupSize = 10;
            int currentLotIndex = 0;

            string screenshotsPath = Application.streamingAssetsPath + "/CianScreenshots";

            while (currentLotIndex < lots.Length)
            {
                Process proc = Bridge.Invoke(Args.Command.ScreenMaker, new ScreenMakerArgs()
                {
                    templatePath = templatePath,
                    screenshotsPath = screenshotsPath,
                    lots = lots.Skip(currentLotIndex).Take(groupSize).ToList(),
                    targetPath = folderSelect.Text + "/",
                });

                currentLotIndex += groupSize;

                processes.Add(proc);

                while(processes.Count >= 10 && processes.All(p => p.HasExited == false))
                {
                    Thread.Sleep(100);
                }
                processes.RemoveAll(p => p.HasExited);
            }
        }

        protected override void OnStop()
        {
        }

        private ScreenMakerLot[] GetLots()
        {
            ScreenMakerLot[] lots = ExcelSerializer.LoadLots<ScreenMakerLot>(tableSelect.FilePath).ToArray();

            var table = ExcelHelper.LoadExcel(Application.streamingAssetsPath + "/CianScreenshots/exceptions.xlsx");

            string[] tableArray = ToArray(table.Tables[0]);

            foreach (ScreenMakerLot lot in lots)
            {
                CalculateScreenshotName(lot, tableArray);
            }

            return lots;
        }
        private void CalculateScreenshotName(ScreenMakerLot lot, string[] tableArray)
        {
            foreach (string except in tableArray)
            {
                Regex regex = new Regex(except + @"\W", RegexOptions.IgnoreCase);

                if (regex.IsMatch(lot.address))
                {
                    lot.screenshot = except;
                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(lot.metro) == false)
            {
                string metroName = lot.metro.Replace("м. ", "");
                int metroBracketIndex = metroName.IndexOf('(');
                metroName = metroName.Substring(0, metroBracketIndex);

                lot.screenshot = metroName;
            }
        }
        private string[] ToArray(ExcelTable table)
        {
            string[] array = new string[table.NumberOfRows - 1];

            for (int y = 2; y <= table.NumberOfRows; y++)
            {
                array[y - 2] = ((string)table.GetValue(y, 1));
            }

            return array;
        }
    }
}