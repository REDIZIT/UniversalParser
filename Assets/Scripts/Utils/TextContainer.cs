using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI.Elements
{
    [RequireComponent(typeof(Text))]
    [ExecuteInEditMode]
    public class TextContainer : MonoBehaviour
    {
        public RectTransform rect;
        private Text text;

        public Vector2 margin;

        public bool fixedHeight, fixedWidth;


        private void Start()
        {
            text = GetComponent<Text>();
        }
        private void Update()
        {
            if (rect == null) return;
            rect.sizeDelta = new Vector2(fixedWidth ? rect.sizeDelta.x : text.preferredWidth + margin.x, fixedHeight ? rect.sizeDelta.y : text.preferredHeight + margin.y);
        }
    }
}