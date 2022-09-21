using System.IO;
using Zenject;

namespace InGame.Dynamics
{
    public class CIANParser : DynamicParser
    {
        private StatusElement status;
        private InputFieldElement folderSelect;
        private IBrowser browser;

        [Inject]
        private void Construct(StatusElement status, InputFieldElement folderSelect, IBrowser browser)
        {
            this.folderSelect = folderSelect;
            this.status = status;
            this.browser = browser;

            status.Setup(new StatusElement.Model()
            {
                parser = this,
                onSwitchWorkStatus = SwitchWorkState
            });
            folderSelect.Setup(new InputFieldElement.Model()
            {
                labelText = "Путь до папки, куда будут выгружены таблицы",
                placeholderText = "Путь до папки",
                onPathChanged = OnTextChanged
            });
        }

        protected override void OnStart()
        {
            status.SetStatus("Открываю браузер");
            browser.Open();
            browser.Driver.Navigate().GoToUrl("https://spb.cian.ru/");
        }
        protected override void OnStop()
        {
            browser.Close();
        }

        private void OnTextChanged()
        {
            folderSelect.SetError(Directory.Exists(folderSelect.Path) == false);
        }
    }
}