using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
	public class ParserPageUI : MonoBehaviour
	{
		[SerializeField] private Text nameText, descriptionText;
		[SerializeField] private Transform content;


		public void Refresh(ParserSO page)
        {
			nameText.text = page.parserName;
			descriptionText.text = page.description;

			if (content.childCount > 1)
            {
				Destroy(content.GetChild(1).gameObject);
            }
			
			
			ParserPage prefab = Instantiate(page.prefabPage, content).GetComponent<ParserPage>();
			prefab.Initialize(page);
        }
	}
}