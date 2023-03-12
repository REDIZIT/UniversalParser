using Newtonsoft.Json;
using RestSharp.Contrib;
using System.IO;
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
                labelText = "Папка для скриншотов",
                placeholderText = "Путь до папки",
                validityCheckFunc = (s) => Directory.Exists(s)
            });

            BakeElements();
        }

        protected override void OnStart()
        {
            string templatePath = '"' + HttpUtility.HtmlEncode(Application.streamingAssetsPath + "/Bridge/template.docx") + '"';

            foreach (ScreenMakerLot lot in ExcelSerializer.LoadLots<ScreenMakerLot>(tableSelect.FilePath))
            {
                string lotJson = '"' + HttpUtility.HtmlEncode(JsonConvert.SerializeObject(lot)) + '"';
                string targetPath = '"' + HttpUtility.HtmlEncode(folderSelect.Text + "/" + lot.id) + '"';
               

                System.Diagnostics.ProcessStartInfo info = new(Application.streamingAssetsPath + "/Bridge/Debug/net6.0/Bridge.exe");
                info.Arguments = templatePath + " " + lotJson + " " + targetPath;
                System.Diagnostics.Process.Start(info);
            }
        }

        protected override void OnStop()
        {
            //throw new System.NotImplementedException();
        }
    }
}