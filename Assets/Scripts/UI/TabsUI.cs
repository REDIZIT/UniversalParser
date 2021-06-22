using InGame.Settings;
using UnityEngine;

namespace InGame.UI
{
	public class TabsUI : MonoBehaviour
	{
        public ParserPageUI parserPageUI;
        public SettingsUI settingsUI;

        [SerializeField] private GameObject settings, allParsers, parserContainer;

        public TabsUI()
        {
            GlobalUI.tabs = this;
        }

		public void ShowSettings()
        {
            LeaveEnableOnlyOne(settings);
            settingsUI.Refresh();
        }

		public void ShowAllParsers()
        {
            LeaveEnableOnlyOne(allParsers);
        }

		public void ShowParserContainer(ParserSO page)
        {
            LeaveEnableOnlyOne(parserContainer);
            parserPageUI.Refresh(page);
        }

        private void LeaveEnableOnlyOne(GameObject toEnable)
        {
            settings.SetActive(toEnable == settings);
            allParsers.SetActive(toEnable == allParsers);
            parserContainer.SetActive(toEnable == parserContainer);
        }
	}
}