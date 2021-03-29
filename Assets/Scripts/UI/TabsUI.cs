using UnityEngine;

namespace InGame.UI
{
	public class TabsUI : MonoBehaviour
	{
        [SerializeField] private GameObject settings, allParsers, parserContainer;

        public TabsUI()
        {
            GlobalUI.tabs = this;
        }

		public void ShowSettings()
        {
            LeaveEnableOnlyOne(settings);
        }

		public void ShowAllParsers()
        {
            LeaveEnableOnlyOne(allParsers);
        }

		public void ShowParserContainer()
        {
            LeaveEnableOnlyOne(parserContainer);
        }

        private void LeaveEnableOnlyOne(GameObject toEnable)
        {
            settings.SetActive(toEnable == settings);
            allParsers.SetActive(toEnable == allParsers);
            parserContainer.SetActive(toEnable == parserContainer);
        }
	}
}