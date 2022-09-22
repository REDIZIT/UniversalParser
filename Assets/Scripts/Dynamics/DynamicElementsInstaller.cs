using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class DynamicElementsInstaller : MonoInstaller
    {
        [SerializeField] private Transform elementsContainer, progressContainer;
        [SerializeField] private GameObject[] elements;
        [SerializeField] private GameObject[] progressElements;
        [SerializeField] private ParserBuilder builder;

        public override void InstallBindings()
        {
            foreach (GameObject go in elements)
            {
                BindElement(go, elementsContainer);
            }
            foreach (GameObject go in progressElements)
            {
                BindElement(go, progressContainer);
            }

            Container.BindInstance(builder);
        }

        private void BindElement(GameObject go, Transform parent)
        {
            IDynamicElement element = go.GetComponent<IDynamicElement>();

            Container.Bind(element.GetType())
                .FromComponentInNewPrefab(go)
                .UnderTransform(parent)
                .AsTransient()
                .Lazy();
        }
    }
}