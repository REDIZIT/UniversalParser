using System;
using UnityEngine;

namespace InGame.Dynamics
{
    public abstract class ParserModel : ScriptableObject, IParserModel
    {
        public string Name => siteName;
        public Sprite Icon => icon;

        [SerializeField] private string siteName;
        [SerializeField] private Sprite icon;

        public abstract Type GetParserType();
    }
}