using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InGame.Dynamics.UI
{
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class ThemedImage : MonoBehaviour
    {
        [SerializeField] private ColorLayer colorLayer;

        private float animationTimeLeft, animationTimeLength;
        private ColorLayer targetColor;
        private Image image;
        private Themes themes;

        [Inject]
        private void Construct(Themes themes)
        {
            this.themes = themes;
            image = GetComponent<Image>();
            image.color = themes.GetColor(colorLayer);
        }
        private void Update()
        {
            if (animationTimeLeft > 0)
            {
                animationTimeLeft -= Time.deltaTime;
                image.color = Color.Lerp(themes.GetColor(targetColor), themes.GetColor(colorLayer), animationTimeLeft / animationTimeLength);

                if (animationTimeLeft <= 0)
                {
                    colorLayer = targetColor;
                }
            }
        }
        private void OnValidate()
        {
            if (themes == null && Application.isPlaying == false)
            {
                // Scene mode in Editor
                themes = Resources.Load<Themes>("Themes");
                Debug.Log("Load themes = " + themes);
            }
            if (image == null) image = GetComponent<Image>();

            image.color = themes.GetColor(colorLayer);
        }
        public void SetColor(ColorLayer layer, float animationTime = 0.1f)
        {
            animationTimeLength = animationTimeLeft = animationTime;
            targetColor = layer;
        }
    }
}