using UnityEngine;

namespace InGame
{
	public class ParserPage : MonoBehaviour
	{
		public new string name;
		public string description;
	}

	[CreateAssetMenu(fileName = "Parser Info SO")]
	public class ParserSO : ScriptableObject
    {
		public string parserName;
		public string description => "�������� ��� ��������� ���������� � ��������� �������� � ������ (��� ��������� ��� ������������) �������";


		[Tooltip("Custom prefab page, if null will be used default")]
		public GameObject prefabPage;


		public ParserSO Clone()
		{
			return MemberwiseClone() as ParserSO;
		}
	}
}