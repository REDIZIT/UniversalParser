using System;
using UnityEngine;

namespace InGame.Dynamics
{
    public abstract class ParserModel : ScriptableObject, IParserModel
    {
        public string Name => siteName;
        public Sprite Icon => icon;
        public string WebsiteUrl => websiteUrl;

        [SerializeField] private string siteName;
        [SerializeField] private Sprite icon;
        [SerializeField] private string websiteUrl;

        public abstract Type GetParserType();
    }
}