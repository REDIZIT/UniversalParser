using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class LegacyDriverWindow : Window<Exception>
    {
        [SerializeField] private Text messageText;

        public LegacyDriverWindow()
        {
            GlobalUI.legacyDriverWindow = this;
        }
        protected override void OnShow(Exception err)
        {
            string supportedVersion = Regex.Match(err.Message, @"(?<=MSEdge version )[^\s]+").Groups[0].Value;
            string currentVersion = Regex.Match(err.Message, @"(?<=browser version is )[^\s]+").Groups[0].Value;

            messageText.text = $"Парсер не поддерживает новую версию Microsoft Edge.\nПарсер поддерживает до {supportedVersion}, а у вас {currentVersion}";
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}