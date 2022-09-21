using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public class StatusElement : DynamicElement<StatusElement.Model>
    {
        [SerializeField] private Text statusText, buttonText;

        public class Model
        {
            public DynamicParser parser;
            public Action onSwitchWorkStatus;
        }

        private void Update()
        {
            buttonText.text = model.parser.IsWorking ? "Остановить" : "Запустить";
        }
        public void SetStatus(string status, string progress = "")
        {
            statusText.text = status + "\r\n<size=10><color=#BBB>" + progress + "</color></size>";
        }
        public void OnClick()
        {
            model.onSwitchWorkStatus?.Invoke();
        }
    }
}