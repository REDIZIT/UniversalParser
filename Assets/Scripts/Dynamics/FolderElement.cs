using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public class FolderElement : DynamicElement<FolderElementModel>
    {
        public string Path => inputField.text;

        [SerializeField] private Text label, placeholderText;
        [SerializeField] private InputField inputField;

        protected override void OnSetup()
        {
            label.text = Model.labelText;
            placeholderText.text = Model.placeholderText;
        }
    }
}