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

        public void Refresh(ParserModel model)
        {
            this.model = model;
            icon.sprite = model.Icon;
            nameText.text = model.Name;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            websiteButton.SetActive(true);
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
    }
}