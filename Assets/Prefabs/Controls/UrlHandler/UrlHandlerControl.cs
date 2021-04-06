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

		private IParser parser;
		private ParseProcess process => parser?.process;

		private Action onUrlHandled;




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
		}


        public void Refresh(IParser parser, Action onUrlHandled)
        {
			Show();
			this.parser = parser;

			if (onUrlHandled != null && process != null) process.onfinished -= onUrlHandled;
			this.onUrlHandled = onUrlHandled;

		}
		public void ClickStart()
		{
			parser.StartParsing(urlField.text);
			process.onfinished += onUrlHandled;
		}


		public void Clear()
        {
			urlField.text = "";
			if (process != null)
            {
				process.onfinished -= onUrlHandled;
			}
        }
    }
}