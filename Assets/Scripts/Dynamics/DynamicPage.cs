using System;
using System.Linq;
using UnityEngine;

namespace InGame.Dynamics
{
    public interface IParserModel
    {
        string Name { get; }
        Sprite Icon { get; }
        Type GetParserType();
    }
    public abstract class DynamicParser
    {
        public abstract void Stop();
    }

    public interface IDynamicElement
    {
        public GameObject gameObject { get; }
    }
    public abstract class DynamicElement<TModel> : MonoBehaviour, IDynamicElement
    {
        protected TModel model;

        public void Setup(TModel model)
        {
            this.model = model;
            OnSetup();
        }

        protected virtual void OnSetup() { }
    }
}