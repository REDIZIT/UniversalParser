using System.IO;
using Zenject;

namespace InGame.Dynamics
{
    public class CIANParser : DynamicParser
    {
        private InputFieldElement folderSelect;

        [Inject]
        private void Construct(InputFieldElement folderSelect)
        {
            this.folderSelect = folderSelect;

            folderSelect.Setup(new InputFieldElement.Model()
            {
                labelText = "���� �� �����, ���� ����� ��������� �������",
                placeholderText = "���� �� �����",
                onPathChanged = OnTextChanged
            });

        }

        public override void Stop()
        {
            //throw new System.NotImplementedException();
        }

        private void OnTextChanged()
        {
            folderSelect.SetError(Directory.Exists(folderSelect.Path) == false);
        }
    }
}