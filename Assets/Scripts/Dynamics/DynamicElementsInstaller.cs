using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class DynamicElementsInstaller : MonoInstaller
    {
        [SerializeField] private Transform elementsContainer, progressContainer, settingsContainer;
        [SerializeField] private GameObject[] settingsElements;
        [SerializeField] private ParserBuilder builder;

        [SerializeField] private GameObject inputField, paging, selectTable, status, folder, option;

        public override void InstallBindings()
        {
            BindElement<IInputField>(inputField, elementsContainer);
            BindElement<IPaging>(paging, elementsContainer);
            BindElement<ISelectTable>(selectTable, elementsContainer);
            BindElement<ISelectFolder>(folder, elementsContainer);
            BindElement<IOption>(option, elementsContainer);

            BindElement<IStatus>(status, progressContainer);

            foreach (GameObject go in settingsElements)
            {
                Container.InstantiatePrefab(go, settingsContainer);
            }

            Container.BindInstance(builder);
        }

        private void BindElement<TInterface>(GameObject go, Transform parent)
        {
            TInterface element = go.GetComponent<TInterface>();

            Container.Bind<TInterface>().To(element.GetType())
                .FromComponentInNewPrefab(go)
                .UnderTransform(parent)
                .AsTransient()
                .Lazy();
        }
    }
}