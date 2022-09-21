using InGame.Dynamics.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public class InputFieldElement : DynamicElement<InputFieldElement.Model>
    {
        public string Path => inputField.text;

        [SerializeField] private Text label, placeholderText;
        [SerializeField] private InputField inputField;
        [SerializeField] private ThemedImage themed;

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

        public void SetError(bool paintAsError)
        {
            themed.SetColor(paintAsError ? ColorLayer.Error : ColorLayer.Button);
        }
    }
}