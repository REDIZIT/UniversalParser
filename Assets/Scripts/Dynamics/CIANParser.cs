using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class CIANParser : DynamicParser
    {
        private FolderElement folderSelect;

        [Inject]
        private void Construct(FolderElement folderSelect)
        {
            this.folderSelect = folderSelect;

            folderSelect.Setup(new FolderElement.Model()
            {
                labelText = "Select folder",
                placeholderText = "Test placeholder",
                onPathChanged = OnTextChanged
            });

        }
        private void OnTextChanged()
        {
            Debug.Log(folderSelect.Path);
        }
    }
}