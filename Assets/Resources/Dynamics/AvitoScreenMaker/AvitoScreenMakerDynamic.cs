using Newtonsoft.Json;
using RestSharp.Contrib;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityParser;
using Zenject;

namespace InGame.Dynamics
{
    public class AvitoScreenMakerDynamic : DynamicParser
    {
        private IInputField folderSelect;
        private ISelectTable tableSelect;
        private IBrowser browser;

        [Inject]
        private void Construct(ISelectTable tableSelect, IInputField folderSelect, IBrowser browser)
        {
            this.tableSelect = tableSelect;
            this.folderSelect = folderSelect;
            this.browser = browser;

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
            string templatePath = Application.streamingAssetsPath + "/AvitoTemplate/index.html";

            browser.Open();
            browser.Maximize();


            string content = File.ReadAllText(templatePath);
            content = Replace(content, "title", "Example title");
            content = Replace(content, "price", "123 000");
            content = Replace(content, "price_per_meter", "1 000");

            string activeTemplatePath = templatePath + ".temp.html";
            File.WriteAllText(activeTemplatePath, content);

            browser.GoToUrl(activeTemplatePath);

            ((Yandex)browser).Screenshot();

            File.Delete(activeTemplatePath);
        }

        protected override void OnStop()
        {
            browser.Close();
        }

        private string Replace(string content, string name, string value)
        {
            return content.Replace("{" + name + "}", value);
        }
    }
}