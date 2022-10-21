using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InGame.Dynamics.UI
{
    public class ParserModelUIItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private Text nameText;
        [SerializeField] private GameObject websiteButton;
        [SerializeField] private ThemedImage themed;

        private ParserModel model;
        private Action<ParserModel> onClick;

        public void Refresh(ParserModel model, Action<ParserModel> onClick)
        {
            this.model = model;
            icon.sprite = model.Icon;
            nameText.text = model.Name;
            this.onClick = onClick;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            websiteButton.SetActive(string.IsNullOrWhiteSpace(model.WebsiteUrl) == false);
            themed.SetColor(ColorLayer.ListItemSelected);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.transform.IsChildOf(transform))
                return;

            websiteButton.SetActive(false);
            themed.SetColor(ColorLayer.ListItemNotSelected);
        }

        public void OnWebsiteClicked()
        {
            Application.OpenURL(model.WebsiteUrl);
        }
        public void OnClick()
        {
            onClick?.Invoke(model);
        }
    }
}