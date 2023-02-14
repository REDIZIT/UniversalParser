using InGame.Dynamics;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public interface IOption : IElement<IOption.Model>
    {
        public Item Selected { get; }

        public class Model : ElementModel
        {
            public string title;
            public List<Item> items;
        }
        public class Item
        {
            public string text;
            public object value;
        }
    }
    public class OptionSelectElement : DynamicElement<IOption.Model>, IOption
    {
        public IOption.Item Selected => itemComponents.Find(i => i.IsToggleOn).Model;

        [SerializeField] private Text title;
        [SerializeField] private OptionSelectUIItem prefab;
        [SerializeField] private ToggleGroup content;

        private List<OptionSelectUIItem> itemComponents;
        private RectTransform rect;

        protected override void OnSetup()
        {
            rect = GetComponent<RectTransform>();
            title.text = model.title;

            bool isFirstItem = true;
            itemComponents = new();
            UIHelper.FillContent<OptionSelectUIItem, IOption.Item>(content.transform, prefab.gameObject, model.items, (item, model) =>
            {
                item.Refresh(model, isFirstItem, content);
                isFirstItem = false;
                itemComponents.Add(item);
            });

            FitSize();
        }

        private void FitSize()
        {
            float minSize = title.rectTransform.sizeDelta.y + 4;
            float perItemSize = prefab.GetComponent<RectTransform>().sizeDelta.y;

            float totalSize = minSize + perItemSize * model.items.Count;
            
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, totalSize);
        }
    }
}