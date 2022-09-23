using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class DynamicElementsInstaller : MonoInstaller
    {
        [SerializeField] private Transform elementsContainer, progressContainer, settingsContainer;
        [SerializeField] private GameObject[] elements, progressElements, settingsElements;
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
            foreach (GameObject go in settingsElements)
            {
                Container.InstantiatePrefab(go, settingsContainer);
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