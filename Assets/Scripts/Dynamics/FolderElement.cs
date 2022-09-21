using System;
using UnityEngine;
using UnityEngine.UI;
using static InGame.Dynamics.FolderElement;

namespace InGame.Dynamics
{
    public class FolderElement : DynamicElement<Model>
    {
        public string Path => inputField.text;

        [SerializeField] private Text label, placeholderText;
        [SerializeField] private InputField inputField;

        public class Model
        {
            public string labelText, placeholderText;
            public Action onPathChanged;
        }

        protected override void OnSetup()
        {
            label.text = model.labelText;
            placeholderText.text = model.placeholderText;
        }

        public void OnTextChanged()
        {
            model.onPathChanged?.Invoke();
        }
    }
}