using InGame.Dynamics.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public interface IInputField : IElement<IInputField.Model>
    {
        string Text { get; }

        public class Model : ElementModel
        {
            public string labelText, placeholderText, defaultText;
            public Action onTextChanged;
            public bool isNumberField;

            /// <summary>Will be invoked on text change. Return <see langword="true"/> if new value is vaild</summary>
            public Func<string, bool> validityCheckFunc;
        }
    }
    public class FakeInputField : IInputField
    {
        public string Text => constantText;
        public GameObject gameObject => null;
        public bool IsValid => true;

        private string constantText;

        public void Setup(IInputField.Model model) { }
        public void Setup(string constantText)
        {
            this.constantText = constantText;
        }
    }
    public class InputFieldElement : DynamicElement<IInputField.Model>, IInputField
    {
        public string Text => inputField.text;

        [SerializeField] private Text label, placeholderText;
        [SerializeField] private InputField inputField;
        [SerializeField] private ThemedImage themed;

        protected override void OnSetup()
        {
            label.text = model.labelText;
            placeholderText.text = model.placeholderText;
            inputField.contentType = model.isNumberField ? InputField.ContentType.DecimalNumber : InputField.ContentType.Standard;
            inputField.text = model.defaultText;
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