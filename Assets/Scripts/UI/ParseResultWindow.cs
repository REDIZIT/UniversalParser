using InGame.Parse;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class ParseResultWindow : Window<ParseProcess>
    {
        [SerializeField] private GameObject successImage, successWarnImage;
        [SerializeField] private Text labelText;

        [SerializeField] private Color successLabelColor, warnLabelColor;

        protected override void OnShow(ParseProcess process)
        {
            if (process.exception == null)
            {
                successImage.SetActive(true);
                successWarnImage.SetActive(false);

                labelText.text = "Обработка страницы завершена";
                labelText.color = successLabelColor;
            }
            else
            {
                successImage.SetActive(false);
                successWarnImage.SetActive(true);

                labelText.text = "Обработка страницы завершена с ошибками";
                labelText.color = warnLabelColor;
            }
        }
    }
}