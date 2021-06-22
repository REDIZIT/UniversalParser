using UnityEditor;
using UnityEngine;

namespace InGame
{
    [CreateAssetMenu(fileName = "Parser Info SO")]
	public class ParserSO : ScriptableObject
    {
		public string parserName;
		public string description => "Собирает всю доступную информацию с поисковой страницы и создаёт (или дополняет уже существующею) таблицу";

		public string parseUIType;

#if UNITY_EDITOR
		public MonoScript parseUI;
#endif

		[Tooltip("Custom prefab page, if null will be used default")]
		public GameObject prefabPage;

#if UNITY_EDITOR
		private void OnValidate()
        {
            if (parseUI != null)
            {
				parseUIType = parseUI.GetClass().FullName;
			}
        }
#endif
		public ParserSO Clone()
		{
			return MemberwiseClone() as ParserSO;
		}
	}
}