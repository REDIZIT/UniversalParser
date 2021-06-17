using InGame.Parse;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityParser;

namespace InGame.UI
{
    public class UrlHandlerControl : Control
	{
		[SerializeField] private GameObject progressGroup;

		[SerializeField] private InputField urlField;
		[SerializeField] private Button startButton;
		[SerializeField] private Slider progressBar;
		[SerializeField] private Text progressText;

		[Header("Current page")]
		[SerializeField] private Text currentPageText;

		[Header("Some pages")]
		[SerializeField] private Toggle enableSomePagesToggle;
		[SerializeField] private InputField startPageField, endPageField;

		private IParser parser;
		private ParseProcess process => parser?.process;

		private Action onUrlHandled, onPageParsed;




		private void Update()
        {
			startButton.interactable = process == null || process.state == ParseProcess.State.Finished;

			if (process == null)
			{
				progressGroup.SetActive(false);
			}
			else
			{
				progressGroup.SetActive(process.state == ParseProcess.State.Running);

				progressBar.gameObject.SetActive(process.state == ParseProcess.State.Running);
				progressBar.value = process.progress;
				progressText.text = process.progressMessage;
			}

			startPageField.interactable = enableSomePagesToggle.isOn;
			endPageField.interactable = enableSomePagesToggle.isOn;



			int number = parser.GetCurrentPageNumberByUrl(urlField.text);
			currentPageText.text = number == -1 ? "-" : number.ToString();
		}


        public void Refresh(IParser parser, Action onUrlHandled, Action onPageParsed)
        {
			Show();
			this.parser = parser;

			if (onUrlHandled != null && process != null)
            {
				process.onfinished -= onUrlHandled;
				process.onPageParsed += onPageParsed;
			}
			this.onUrlHandled = onUrlHandled;
			this.onPageParsed = onPageParsed;
		}
		public void ClickStart()
		{
            int.TryParse(startPageField.text, out int startPage);
            int.TryParse(endPageField.text, out int endPage);

            parser.StartParsing(urlField.text, enableSomePagesToggle.isOn ? startPage : 0, endPage);
			process.onfinished += onUrlHandled;
			process.onPageParsed += onPageParsed;
		}
		public void ClickNextPage()
        {
			int pageNumber = parser.GetCurrentPageNumberByUrl(urlField.text);
			pageNumber++;

			urlField.text = parser.GetUrlWithPageNumber(urlField.text, pageNumber);
        }
		public void ClickPrevPage()
        {
			int pageNumber = parser.GetCurrentPageNumberByUrl(urlField.text);
			pageNumber--;

			urlField.text = parser.GetUrlWithPageNumber(urlField.text, pageNumber);
		}

		public void Clear()
        {
			urlField.text = "";
			if (process != null)
            {
				process.onfinished -= onUrlHandled;
				process.onPageParsed -= onPageParsed;


			}
        }
    }
}