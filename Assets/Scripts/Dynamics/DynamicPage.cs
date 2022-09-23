using System;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace InGame.Dynamics
{
    public interface IParserModel
    {
        string Name { get; }
        Sprite Icon { get; }
        Type GetParserType();
    }

    public interface IDynamicElement
    {
        public GameObject gameObject { get; }
        public bool IsValid { get; }
    }
}