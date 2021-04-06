using InGame.Parse;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityParser;

namespace InGame.UI
{
    public class SummaryControl : MonoBehaviour
	{
		public SelectTableControl selectTableUI;

		[SerializeField] private Transform content;
		[SerializeField] private SummaryUIItem itemPrefab;
		[SerializeField] private GameObject separatorPrefab;

		[SerializeField] private Text pagesCountText, newPositionsCountText, errorsCountText;

		private List<ParseResult> results = new List<ParseResult>();
		private bool isSaved = true;
		private bool forceQuit;

		private Action onSaveClick;


        private void Awake()
        {
			Application.wantsToQuit += () =>
			{
				if (forceQuit || isSaved) return true;

				GlobalUI.notSafeExitWindow.Show(new NotSafeExitWindow.Arguments()
				{
					 onExitWithoutSave = () => { forceQuit = true; Application.Quit(); },
					 onSaveAndExit = () => { ClickSave(); forceQuit = true; Application.Quit(); }
				});

				return false;
			};
		}
        public void OnParseFinished(ParseProcess process, Action onSaveClick)
        {
			if (process.result == null) return;

			this.onSaveClick = onSaveClick;
			results.Add(process.result);

			isSaved = false;

			RefreshContent();
		}
		public void ClearResults()
        {
			results.Clear();
			RefreshContent();
		}

		public void ClickSave()
        {
			isSaved = false;

			try
            {
				onSaveClick?.Invoke();
			}
            catch(Exception err)
            {
				GlobalUI.closeExcelWindow.Show(err);
				Debug.LogError(err);
			}

			isSaved = true;
		}

		

		private void RefreshContent()
		{
			ClearChildren(content);

			int i = 0;
			foreach (ParseResult result in results)
			{
				GameObject inst = Instantiate(itemPrefab.gameObject, content);
				SummaryUIItem uii = inst.GetComponent<SummaryUIItem>();
				uii.Refresh(result, i);

				i++;
			}

			pagesCountText.text = "Страниц обработано: " + i;
			newPositionsCountText.text = "Новых позиций: " + results.Sum(r => r.lots.Count(l => l.exception == null));
			errorsCountText.text = "Ошибок: " + results.Sum(r => r.lots.Count(l => l.exception != null));
		}
		private void ClearChildren(Transform content)
        {
            foreach (Transform child in content)
            {
				Destroy(child.gameObject);
            }
		}
	}
}