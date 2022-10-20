using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Dynamics
{
    public interface IStatus : IElement<IStatus.Model>
    {
        public string Status { get; set; }
        public string Progress { get; set; }

        public class Model : ElementModel
        {
            public DynamicParser parser;
            public Action onSwitchWorkStatus;

            public Model(DynamicParser parser)
            {
                this.parser = parser;
            }
        }
    }
    public class StatusElement : DynamicElement<IStatus.Model>, IStatus
    {
        public string Status { get; set; }
        public string Progress { get; set; }


        [SerializeField] private Text statusText, buttonText;
        [SerializeField] private Button startButton;

        private void Update()
        {
            buttonText.text = model.parser.IsWorking ? "Остановить" : "Запустить";
            statusText.text = Status + "\r\n<size=10><color=#BBB>" + Progress + "</color></size>";
            startButton.interactable = ActiveParser.IsReadyToStart();
        }
        public void OnClick()
        {
            model.onSwitchWorkStatus?.Invoke();
        }
    }
}