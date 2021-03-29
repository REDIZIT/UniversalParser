using InGame.UI.Items;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
	public class ParsersListUI : MonoBehaviour
	{
		[SerializeField] private Transform content;
		[SerializeField] private ParserPageUII prefab;

        private void Start()
        {
            UIHelper.FillContent<ParserPageUII, ParserPage>(content, prefab.gameObject, ParsersStorage.pages, (uii, page) =>
            {
                uii.Refresh(page, OnPageClicked);
            });
        }

        private void OnPageClicked(ParserPage page)
        {
            GlobalUI.tabs.ShowParserContainer();
        }
    }
}