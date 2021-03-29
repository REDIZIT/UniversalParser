using UnityEngine;
using UnityEngine.UI;

namespace InGame.Themes
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(InputField), typeof(Image))]
	public class ThemeInputField : MonoBehaviour
	{
        private InputField field;
        private Image image;

        private void Awake()
        {
            field = GetComponent<InputField>();
            image = GetComponent<Image>();
        }
        private void Start()
        {
            image.sprite = ThemeManager.instance.inputFieldBackground;
            image.color = ThemeManager.instance.inputFieldColor;
        }

        private void Update()
        {
            if (Application.isPlaying == false && Application.isEditor)
            {
                Start();
            }
        }
    }
}