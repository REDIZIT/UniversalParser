using System.IO;
using System.Threading;
using Bridge;
using UnityEngine;
using Xceed.Document.NET;
using Xceed.Words.NET;
using Zenject;

namespace InGame.Dynamics
{
    public class AvitoFullScreenshooter : DynamicParser
    {
        private IInputField tableField;
        private IInputField screenshotsFolder;
        private IInputField delayField;
        private IBrowser browser;

        [Inject]
        private void Construct(IInputField tableField, IInputField screenshotsFolder, IInputField delayField, IBrowser browser)
        {
            this.tableField = tableField;
            this.screenshotsFolder = screenshotsFolder;
            this.delayField = delayField;
            this.browser = browser;

            tableField.Setup(new IInputField.Model()
            {
                labelText = "Таблица Excel со ссылками",
                placeholderText = "Путь до файла Excel",
                validityCheckFunc = File.Exists
            });

            screenshotsFolder.Setup(new IInputField.Model()
            {
                labelText = "Папка для выгрузки скриншотов",
                placeholderText = "Путь до папки",
                validityCheckFunc = Directory.Exists
            });

            delayField.Setup(new IInputField.Model()
            {
                labelText = "Задержка в секундах между переходами по ссылкам",
                isNumberField = true,
                placeholderText = "Время в секундах",
                defaultText = "3",
                validityCheckFunc = (s) => int.TryParse(s, out int i) && i >= 0
            });

            BakeElements();
        }

        protected override void OnStart()
        {
            browser.Open();
            browser.Maximize();

            Yandex yandex = (Yandex) browser;

            var table = ExcelHelper.LoadExcel(tableField.Text).Tables[0];

            int delay = int.Parse(delayField.Text);

            status.Progress = "0 / " + table.NumberOfRows;

            for (int i = 1; i < table.NumberOfRows; i++)
            {
                status.Progress = i + " / " + table.NumberOfRows;

                string url = table.GetCell(1 + i, 1).Value;

                string[] split = url.Split("_");
                string lotID = split[split.Length - 1];

                status.Status = "Скачиваю страницу";
                yandex.GoToUrl(url);

                status.Status = "Перерыв " + delay + " сек.";
                Thread.Sleep(delay * 1000);

                status.Status = "Делаю скриншот";
                string filename = screenshotsFolder.Text + "/" + lotID;
                yandex.ScreenshotFullPage( filename + ".png");


                var proc = Bridge.Invoke(Args.Command.ImageAndUrlToPDF, new ImageAndUrlToPDFModel()
                {
                    imagePath = filename + ".png",
                    url = url,
                    pdfPath = filename + ".pdf"
                });
                proc.WaitForExit();
            }
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