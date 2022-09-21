using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class DynamicElementsInstaller : MonoInstaller
    {
        [SerializeField] private Transform elementsContainer;
        [SerializeField] private GameObject[] elements;

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
        }
    }
}