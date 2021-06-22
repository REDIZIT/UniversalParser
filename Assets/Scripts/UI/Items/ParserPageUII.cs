using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI.Items
{
	public class ParserPageUII : MonoBehaviour
	{
		[SerializeField] private Text text;

		private ParserSO page;
		private Action<ParserSO> onclick;

		public void Refresh(ParserSO page, Action<ParserSO> onclick)
        {
			this.page = page;
			this.onclick = onclick;

			text.text = page.parserName;
        }

		public void Click()
        {
			onclick?.Invoke(page);
		}
	}
}