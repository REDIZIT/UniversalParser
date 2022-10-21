using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public class SelectFolderElement : DynamicElement<ISelectFolder.Model>, ISelectFolder
    {
        public string Path { get; private set; }

        [SerializeField] private InputField field;

        protected override void OnSetup()
        {
            base.OnSetup();

            field.onValueChanged.AddListener(p =>
            {
                IsValid = Directory.Exists(p);
                Path = IsValid ? p : null;
            });
        }
    }
}