using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
	public class ParserPageUI : MonoBehaviour
	{
		[SerializeField] private Text nameText, descriptionText;
		[SerializeField] private Transform content;

		private ParserPage page;

		public void Refresh(ParserPage page)
        {
			this.page = page;

			nameText.text = page.name;
			descriptionText.text = page.description;

			if (content.childCount > 1)
            {
				Destroy(content.GetChild(1).gameObject);
            }
			GameObject go = Instantiate(page.gameObject, content);
        }
	}
}