using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class CIANParser : DynamicParser
    {
        private FolderElement folderSelect;

        [Inject]
        private void Construct(FolderElement f1, FolderElement folderSelect, FolderElement f2, FolderElement f3)
        {
            this.folderSelect = folderSelect;

            f1.Setup(new FolderElementModel()
            {
                labelText = "1"
            });


            folderSelect.Setup(new FolderElementModel()
            {
                labelText = "Select folder",
                placeholderText = "Test placeholder"
            });

           
            f2.Setup(new FolderElementModel()
            {
                labelText = "2"
            });

            f3.Setup(new FolderElementModel()
            {
                labelText = "3"
            });

        }
        public void Start()
        {
            Debug.Log(folderSelect.Path);
        }
    }
}