using UnityEngine;

namespace InGame.Dynamics
{
    public abstract class DynamicElement<TModel> : MonoBehaviour, IDynamicElement
    {
        protected TModel model;


        private void Start()
        {
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