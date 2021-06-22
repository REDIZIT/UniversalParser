using InGame.UI.Items;
using UnityEngine;

namespace InGame.UI
{
	public class ParsersListUI : MonoBehaviour
	{
		[SerializeField] private Transform content;
		[SerializeField] private ParserPageUII prefab;

        private void Start()
        {
            UIHelper.FillContent<ParserPageUII, ParserSO>(content, prefab.gameObject, ParsersStorage.parsersInfo, (uii, info) =>
            {
                uii.Refresh(info, OnPageClicked);
            });
        }

        private void OnPageClicked(ParserSO page)
        {
            GlobalUI.tabs.ShowParserContainer(page);
        }
    }
}