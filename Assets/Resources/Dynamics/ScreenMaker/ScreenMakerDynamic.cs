using Newtonsoft.Json;
using RestSharp.Contrib;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityParser;
using Zenject;

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

            ScreenMakerLot[] lots = ExcelSerializer.LoadLots<ScreenMakerLot>(tableSelect.FilePath).ToArray();

            List<Process> processes = new List<Process>();

            int groupSize = 10;
            int currentLotIndex = 0;

            string screenshotsPath = Application.streamingAssetsPath + "/CianScreenshots";

            while (currentLotIndex < lots.Length)
            {
                ProcessStartInfo info = new(Application.streamingAssetsPath + "/Bridge/Debug/net6.0/Bridge.exe");

                Args args = new Args()
                {
                    templatePath = templatePath,
                    screenshotsPath = screenshotsPath,
                    lots = lots.Skip(currentLotIndex).Take(groupSize).ToList(),
                    targetPath = folderSelect.Text + "/",
                };

                currentLotIndex += groupSize;

                info.Arguments = '"' + HttpUtility.HtmlEncode(JsonConvert.SerializeObject(args)) + '"';

                var proc = Process.Start(info);
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

        public class Args
        {
            public string templatePath;
            public string screenshotsPath;
            public List<ScreenMakerLot> lots;
            public string targetPath;
        }
    }
}