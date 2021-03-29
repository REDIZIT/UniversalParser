using System.Collections.Generic;
using UnityEngine;
using UnityParser;

namespace InGame
{
	public static class ParsersStorage 
	{
		public static List<ParserPage> pages;


		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
        {
			pages = new List<ParserPage>();

            foreach (GameObject prefab in Resources.LoadAll<GameObject>("Parsers"))
            {
				ParserPage page = prefab.GetComponent<ParserPage>();
				if (page != null)
                {
					pages.Add(page);
                }
            }
        }
	}
}