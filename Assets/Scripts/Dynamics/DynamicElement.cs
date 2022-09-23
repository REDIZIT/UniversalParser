using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public abstract class DynamicElement<TModel> : MonoBehaviour, IDynamicElement
    {
        public bool IsValid { get; protected set; } = true;

        protected DynamicParser ActiveParser => builder.ActiveParser;
        protected TModel model;

        private ParserBuilder builder;

        [Inject]
        private void Construct(ParserBuilder builder)
        {
            this.builder = builder;
        }

        private void Start()
        {
            ActiveParser.Elements.Add(this);

            if (model == null)
            {
                Debug.LogError($"You need to setup DynamicElement ({name}) before Start event");
            }
        }

        public void Setup(TModel model)
        {
            this.model = model;
            OnSetup();
        }
        protected virtual void OnSetup() { }
    }
}