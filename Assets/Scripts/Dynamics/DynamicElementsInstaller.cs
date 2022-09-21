using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class DynamicElementsInstaller : MonoInstaller
    {
        [SerializeField] private Transform elementsContainer, progressContainer;
        [SerializeField] private GameObject[] elements;
        [SerializeField] private GameObject[] progressElements;

        public override void InstallBindings()
        {
            foreach (GameObject go in elements)
            {
                IDynamicElement element = go.GetComponent<IDynamicElement>();

                Container.Bind(element.GetType())
                    .FromComponentInNewPrefab(go)
                    .UnderTransform(elementsContainer)
                    .AsTransient()
                    .Lazy();
            }

            foreach (GameObject go in progressElements)
            {
                IDynamicElement element = go.GetComponent<IDynamicElement>();

                Container.Bind(element.GetType())
                    .FromComponentInNewPrefab(go)
                    .UnderTransform(progressContainer)
                    .AsTransient()
                    .Lazy();
            }
        }
    }

}