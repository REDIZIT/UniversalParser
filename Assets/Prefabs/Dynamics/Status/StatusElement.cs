using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InGame.Dynamics
{
    public class StatusElement : DynamicElement<StatusElement.Model>
    {
        public string Status { get; set; }
        public string Progress { get; set; }


        [SerializeField] private Text statusText, buttonText;
        [SerializeField] private Button startButton;

        public class Model
        {
            public DynamicParser parser;
            public Action onSwitchWorkStatus;

            public Model(DynamicParser parser)
            {
                this.parser = parser;
            }
        }

        private void Update()
        {
            buttonText.text = model.parser.IsWorking ? "Остановить" : "Запустить";
            statusText.text = Status + "\r\n<size=10><color=#BBB>" + Progress + "</color></size>";
            startButton.interactable = ActiveParser.Elements.All(e => e.IsValid);
        }
        public void OnClick()
        {
            model.onSwitchWorkStatus?.Invoke();
        }
    }
}