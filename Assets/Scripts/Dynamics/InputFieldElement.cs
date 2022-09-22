using InGame.Dynamics.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public class InputFieldElement : DynamicElement<InputFieldElement.Model>
    {
        public string Text => inputField.text;

        [SerializeField] private Text label, placeholderText;
        [SerializeField] private InputField inputField;
        [SerializeField] private ThemedImage themed;

        public class Model
        {
            public string labelText, placeholderText;
            public Action onTextChanged;

            /// <summary>Will be invoked on text change. Return <see langword="true"/> if new value is vaild</summary>
            public Func<string, bool> validityCheckFunc;
        }

        protected override void OnSetup()
        {
            label.text = model.labelText;
            placeholderText.text = model.placeholderText;
            CheckValidity();
        }

        public void OnTextChanged()
        {
            model.onTextChanged?.Invoke();
            CheckValidity();
        }

        private void CheckValidity()
        {
            IsValid = model.validityCheckFunc == null ? true : model.validityCheckFunc.Invoke(Text);
            themed.SetColor(IsValid ? ColorLayer.Button : ColorLayer.Error);
        }
    }
}