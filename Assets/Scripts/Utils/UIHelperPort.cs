using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace InGame
{
    public class UIHelperPort
    {
        private DiContainer container;

        [Inject]
        private void Construct(DiContainer container)
        {
            this.container = container;
        }

        public void FillContent<TUIItem, TList>(Transform content, TUIItem prefab, IEnumerable<TList> list, Action<TUIItem, TList> forEachFunc) where TUIItem : MonoBehaviour
        {
            UIHelper.FillContent(content, prefab.gameObject, list, forEachFunc, Instantiate);
        }

        private GameObject Instantiate(GameObject prefab, Transform parent)
        {
            return container.InstantiatePrefab(prefab, parent);
        }
    }
}