using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class SavedWindow : Window<string>
    {
        [SerializeField] private Text messageText;

        public SavedWindow()
        {
            GlobalUI.savedWindow = this;
        }

        protected override void OnShow(string filepath)
        {
            messageText.text = "Путь до таблицы: " + filepath;
        }
    }
}