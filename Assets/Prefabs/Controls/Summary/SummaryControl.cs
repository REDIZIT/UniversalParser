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
		public SelectTableElement selectTableUI;

		[SerializeField] private Transform content;
		[SerializeField] private SummaryUIItem itemPrefab;
		[SerializeField] private GameObject separatorPrefab;

		[SerializeField] private Text pagesCountText, newPositionsCountText, errorsCountText;

        private List<IParseResult> results = new List<IParseResult>();
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
		public void OnPageParsed(IParser parser, Action onSaveClick)
		{
			if (parser == null || parser.process == null || parser.process.bigResult == null) return;

			this.onSaveClick = onSaveClick;
			results.Add(parser.process.currentPageResult);

			isSaved = false;

			RefreshContent();
		}

		public void OnParseFinished(IParser parser, Action onSaveClick)
        {
			if (parser == null || parser.process == null || parser.process.bigResult == null) return;

			this.onSaveClick = onSaveClick;
			results.AddRange(parser.process.results);

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
		public IParseResult GetBigResult()
        {
			if (results.Count == 0) return new ParseResult<Lot>();

			IParseResult bigResult = results[0].Clone();
            bigResult.Clear();
            foreach (var item in results)
            {
				bigResult.MergeWith(item);
            }

			var uniqueLots = bigResult.EnumerateLots().DistinctBy(l => l.url);
            bigResult.RemoveWhere(t => uniqueLots.Contains(t) == false);

			return bigResult;
        }
		

		private void RefreshContent()
		{
			ClearChildren(content);

			UIHelper.ClearChildren(content);

			//if (parser == null || parser.process == null) return;

			int i = 0;
			UIHelper.FillContent<SummaryUIItem, IParseResult>(content, itemPrefab.gameObject, results, (uii, r) =>
			{
				uii.Refresh(r, i);
				i++;
			});

			var bigResult = GetBigResult();

			pagesCountText.text = "Страниц обработано: " + results.Count;
			newPositionsCountText.text = "Новых позиций: " + bigResult.EnumerateLots().Count(l => l.exception == null);
			errorsCountText.text = "Ошибок: " + bigResult.EnumerateLots().Count(l => l.exception != null);
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