using System;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI.Items
{
	public class ParserPageUII : MonoBehaviour
	{
		[SerializeField] private Text text;

		private ParserPage page;
		private Action<ParserPage> onclick;

		public void Refresh(ParserPage page, Action<ParserPage> onclick)
        {
			this.page = page;
			this.onclick = onclick;

			text.text = page.name;
        }

		public void Click()
        {
			onclick?.Invoke(page);
		}
	}
}