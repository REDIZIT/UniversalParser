using UnityEngine;

namespace InGame.UI
{
    public class Control : MonoBehaviour
    {
		public void Show()
		{
			gameObject.SetActive(true);
		}
		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}