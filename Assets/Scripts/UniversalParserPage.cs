using UnityEngine;

namespace InGame
{
    public class UniversalParserPage : MonoBehaviour
    {
		public new string name;
		public string description;

		public virtual void Initialize(ParserSO parserInfo) { }
	}
}