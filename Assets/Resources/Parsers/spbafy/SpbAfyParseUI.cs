using InGame.Dynamics;
using InGame.Parse;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class SpbAfyParseUI : MonoBehaviour
    {
        [SerializeField] private SelectTableElement importTableControl, exportTableControl;
        [SerializeField] private InputField urlField, pagesField;
        [SerializeField] private Button startButton, cancelButton;
        [SerializeField] private Text currentPageText;

        private SpbAfyParser parser = new SpbAfyParser();
        private int pagesCount;

        private void Update()
        {
            startButton.gameObject.SetActive(parser.IsWorking == false);
            startButton.interactable = importTableControl.IsSelected && exportTableControl.IsSelected && string.IsNullOrEmpty(urlField.text) == false;
            cancelButton.gameObject.SetActive(startButton.gameObject.activeSelf == false);

            urlField.interactable = parser.IsWorking == false;
            pagesField.interactable = parser.IsWorking == false;

            if (int.TryParse(pagesField.text, out int page))
            {
                pagesCount = page;
                if (page <= 0 && string.IsNullOrWhiteSpace(pagesField.text) == false)
                {
                    pagesField.text = "1";
                }
            }

            if (parser.IsWorking)
            {
                currentPageText.text = "Текущая страница: " + parser.CurrentPage;
            }
            else
            {
                currentPageText.text = "";
            }
        }
        private void OnApplicationQuit()
        {
            parser.Abort();
        }
        public void ClickStart()
        {
            parser.StartParsing(urlField.text);
            parser.process.onfinished += () => { };
            parser.process.onPageParsed += OnFinished;

            parser.SetImportResults(importTableControl.Load<MirkvartirLot>());
            parser.SetPagesCount(pagesCount);
        }
        public void ClickCancel()
        {
            parser.Abort();
        }

        private void OnFinished()
        {
            GlobalUI.parseResultWindow.Show(parser.process);
            exportTableControl.SaveResult(parser.process.bigResult);
        }
    }
}