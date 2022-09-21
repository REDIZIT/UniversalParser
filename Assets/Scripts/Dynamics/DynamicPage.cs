using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGame.Dynamics
{
    public class ParsersView : MonoBehaviour
    {
        public List<IParserModel> parsers = new List<IParserModel>();

        [SerializeField] private ParserBuilder builder;

        public void Refresh()
        {

        }

        public void OnParserSelected(IParserModel model)
        {
            builder.Build(model);
        }
    }
    public class ParserBuilder : MonoBehaviour
    {
        [SerializeField] private Transform elementsContainer;

        public void Build(IParserModel model)
        {
            DynamicParser instance = (DynamicParser)Activator.CreateInstance(model.GetParserType());

            foreach (IDynamicElement element in model.Elements)
            {
                IDynamicElement elementInst = Instantiate(element.gameObject, elementsContainer).GetComponent<IDynamicElement>();

                //instance.RegisterElement(elementInst);
            }
        }
    }
    public interface IParserModel
    {
        string Name { get; }
        Sprite Icon { get; }
        IDynamicElement[] Elements { get; }
        Type GetParserType();
    }
    public abstract class ParserModel<T> : ScriptableObject, IParserModel where T : DynamicParser
    {
        public string Name => siteName;
        public Sprite Icon => icon;
        public IDynamicElement[] Elements => elements;

        [SerializeField] private string siteName;
        [SerializeField] private Sprite icon;
        [SerializeField] private IDynamicElement[] elements;

        public Type GetParserType()
        {
            return GetType().GetGenericArguments()[0];
        }
    }
    public class CIANModel : ParserModel<CIANParser>
    {
    }
    public abstract class DynamicParser : MonoBehaviour
    {

    }



    public abstract class DynamicElementModel<TInstance> : ScriptableObject
    {

    }
    public interface IDynamicElement
    {
        public GameObject gameObject { get; }
    }
    public abstract class DynamicElement<TModel> : MonoBehaviour, IDynamicElement
    {
        protected TModel Model { get; private set; }

        public void Setup(TModel model)
        {
            Model = model;
            OnSetup();
        }

        protected virtual void OnSetup() { }
    }





    public class FolderElementModel : DynamicElementModel<FolderElement>
    {
        public string labelText, placeholderText;
    }
}