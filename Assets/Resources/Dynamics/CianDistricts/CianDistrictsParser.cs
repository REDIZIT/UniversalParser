using System.IO;
using Zenject;

namespace InGame.Dynamics
{
    public class CianDistrictsParser : DynamicParser
    {
        private IInputField url, folder;
        private IWebClient c;

        [Inject]
        private void Construct(IInputField url, IInputField folder, IWebClient c)
        {
            this.url = url;
            this.folder = folder;
            this.c = c;

            url.Setup(new()
            {
                labelText = "Ссылка с фильтрами",
                placeholderText = "Ссылка на cian"
            });
            folder.Setup(new()
            {
                labelText = "Выгрузить таблицы в папку",
                placeholderText = "Путь до папки",
                validityCheckFunc = (s) => Directory.Exists(s)
            });

            BakeElements();
        }

        protected override void OnStart()
        {
            Excel sourceTable = ExcelHelper.LoadExcel(Pathes.steamingAssets + "/source_table.xlsx");
            var table = sourceTable.Tables[0];

            for (int i = 1; i <= table.NumberOfRows; i++)
            {
                int cellRow = i + 1;
                var cell = table.GetCell(cellRow, 1);
                if (cell == null) continue;

                var nameCell = table.GetCell(cellRow, 2);

                string districtNo = cell.Value;
                string districtName = nameCell.Value;

                string url = this.url.Text + $"&district%5B0%5D={districtNo}";

                string downloadUrl = url.Replace("cat.php", "export/xls/offers/");
                string targetFileName = folder.Text + "/" + districtName + ".xlsx";
                c.DownloadFile(downloadUrl, targetFileName);
            }


            

            //
        }

        protected override void OnStop()
        {
        }
    }
}