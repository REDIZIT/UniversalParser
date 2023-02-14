using UnityEngine;
using UnityEngine.UI;

namespace InGame
{
    public class OptionSelectUIItem : MonoBehaviour
    {
        public IOption.Item Model { get; private set; }
        public bool IsToggleOn => toggle.isOn;

        [SerializeField] private Toggle toggle;
        [SerializeField] private Text text;

        public void Refresh(IOption.Item item, bool isSelected, ToggleGroup toggleGroup)
        {
            Model = item;
            text.text = item.text;
            toggle.group = toggleGroup;
            toggle.isOn = isSelected;
        }
    }
}