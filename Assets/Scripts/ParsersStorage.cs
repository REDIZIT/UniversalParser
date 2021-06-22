using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
	public static class ParsersStorage 
	{
		public static List<ParserSO> parsersInfo;

		private static ParserPage defaultPage;
		private static List<ParserPage> pages;



		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
        {
			pages = new List<ParserPage>();
			parsersInfo = new List<ParserSO>();


			foreach (GameObject prefab in Resources.LoadAll<GameObject>("Parsers"))
            {
				ParserPage page = prefab.GetComponent<ParserPage>();
				if (page != null)
                {
					pages.Add(page);
					if (page.name == "Парсер avito")
                    {
						defaultPage = page;
                    }
                }
            }

            foreach (ParserSO info in Resources.LoadAll<ParserSO>("Parsers"))
            {
				ParserSO so = info.Clone();

				if (so.prefabPage == null)
                {
					so.prefabPage = defaultPage.gameObject;
                }

				parsersInfo.Add(so);
            }
        }
	}
}