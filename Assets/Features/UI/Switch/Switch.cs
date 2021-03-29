using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InGame.UI.Elements
{
	[RequireComponent(typeof(Slider))]
	public class Switch : MonoBehaviour, IPointerDownHandler 
    {
        public bool isOn { get; protected set; }
        public Action<bool> onValueChanged { get; set; }

        [SerializeField] private Image background, handle;
        [SerializeField] private Color backgroundDisabled, backgroundEnabled;
        [SerializeField] private Color handleDisabled, handleEnabled;

		private Slider slider;
        private Coroutine animationCoroutine;

        private void Awake()
        {
            slider = GetComponent<Slider>();
            ForceAnimation(isOn);
        }

        public void SetIsOn(bool isOn, bool ignoreAnimation)
        {
            this.isOn = isOn;

            if (animationCoroutine != null)
                StopCoroutine(animationCoroutine);


            if (ignoreAnimation)
            {
                ForceAnimation(isOn);
            }
            else
            {
                animationCoroutine = StartCoroutine(Animate(isOn));
                onValueChanged?.Invoke(isOn);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SetIsOn(!isOn, false);
        }

        private IEnumerator Animate(bool turnOn)
        {
            float sliderStartValue = turnOn ? 0 : 1;
            float sliderTargetValue = turnOn ? 1 : 0;

            Color startBackgroundColor = turnOn ? backgroundDisabled : backgroundEnabled;
            Color targetBackgroundColor = turnOn ? backgroundEnabled : backgroundDisabled;

            Color startHandleColor = turnOn ? handleDisabled : handleEnabled;
            Color targetHandleColor = turnOn ? handleEnabled : handleDisabled;

            float animTime = .12f;
            float currentAnimTime = 0;
            while(currentAnimTime < animTime)
            {
                currentAnimTime += Time.unscaledDeltaTime;
                float t = currentAnimTime / animTime;

                background.color = Color.Lerp(startBackgroundColor, targetBackgroundColor, t);
                handle.color = Color.Lerp(startHandleColor, targetHandleColor, t);
                slider.value = Mathf.Lerp(sliderStartValue, sliderTargetValue, t);

                yield return null;
            }
        }
        private void ForceAnimation(bool turnOn)
        {
            slider.value = turnOn ? 1 : 0;

            background.color = turnOn ? backgroundEnabled : backgroundDisabled;
            handle.color = turnOn ? handleEnabled : handleDisabled;
        }
    }
}