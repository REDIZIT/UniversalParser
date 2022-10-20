using InGame.Dynamics;
using InGame.UI;
using System;
using System.Reflection;
using UnityEngine;

namespace InGame
{
    public class ParserPage : UniversalParserPage
	{
		public SelectTableElement selectTableUI;
		public UrlHandlerControl urlControl;
		public SummaryControl summary;

        public override void Initialize(ParserSO parserInfo)
        {
            base.Initialize(parserInfo);

			Type classType = Assembly.GetExecutingAssembly().GetType(parserInfo.parseUIType);
			MonoBehaviour mono = Activator.CreateInstance(classType) as MonoBehaviour;
			gameObject.AddComponent(mono.GetType());
		}
	}
}