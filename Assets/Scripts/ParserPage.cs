using InGame.UI;
using System;
using System.Reflection;
using UnityEngine;

namespace InGame
{
    public class ParserPage : MonoBehaviour
	{
		public new string name;
		public string description;


		public SelectTableControl selectTableUI;
		public UrlHandlerControl urlControl;
		public SummaryControl summary;


		public void Initialize(ParserSO parserInfo)
        {
			Type classType = Assembly.GetExecutingAssembly().GetType(parserInfo.parseUIType);
			MonoBehaviour mono = Activator.CreateInstance(/*parserInfo.parseUI.GetClass()*/classType) as MonoBehaviour;
			gameObject.AddComponent(mono.GetType());
		}
	}
}