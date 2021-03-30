using InGame.Parse;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
	public class ParseUI : MonoBehaviour
	{
		public ParseManager parse;
		public SelectTableUI selectTableUI;
		public SummaryUI summary;

		[Header("UI")]
		[SerializeField] private GameObject group;

		[SerializeField] private GameObject progressGroup;

		[SerializeField] private InputField urlField;
		[SerializeField] private Button startButton;
		[SerializeField] private Slider progressBar;
		[SerializeField] private Text progressText;

		private ParseProcess process;


        private void Update()
        {
			group.SetActive(selectTableUI.workingTableType != SelectTableUI.WorkingTableType.NotSelected);

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


		public void Clear()
        {
			urlField.text = "";
			process = null;
			summary.ClearResults();
		}

		public void ClickStart()
        {
			process = parse.StartParsing(urlField.text);
			process.onfinished += OnParseFinished;
		}

		private void OnParseFinished()
        {
			GlobalUI.parseResultWindow.Show(process);
			summary.OnParseFinished(process);
		}
    }
}