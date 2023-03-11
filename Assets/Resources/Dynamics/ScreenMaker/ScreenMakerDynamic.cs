using UnityEngine;
using UnityParser;
using Zenject;

namespace InGame.Dynamics
{
    public class ScreenMakerDynamic : DynamicParser
    {
        //private IBrowser browser;
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
                labelText = "����� ��� ����������",
                placeholderText = "���� �� �����",
                validityCheckFunc = (s) => string.IsNullOrWhiteSpace(s)
            });

            BakeElements();
        }

        protected override void OnStart()
        {
            Debug.Log("Load table");
            foreach (ScreenMakerLot lot in ExcelSerializer.LoadLots<ScreenMakerLot>(tableSelect.FilePath))
            {
                Debug.Log("Lift: " + lot.lift);
            }

            //System.Diagnostics.ProcessStartInfo info = new(Application.streamingAssetsPath + "/Bridge/Debug/net6.0/Bridge.exe");

            //List

            //info.Arguments = Application.streamingAssetsPath + "/Bridge/template.docx" + " 123123";
            //System.Diagnostics.Process.Start(info);
        }

        protected override void OnStop()
        {
            //throw new System.NotImplementedException();
        }
    }
}