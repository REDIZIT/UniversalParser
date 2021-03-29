using System.Collections;
using UnityEngine;

namespace InGame.UI
{

    [RequireComponent(typeof(Animator))]
	public abstract class Window<T> : MonoBehaviour
	{
		private Animator animator;

        private void Awake()
        {
			animator = GetComponent<Animator>();
        }

        public void Show(T argument)
        {
			gameObject.SetActive(true);
			animator.Play("WindowShow");

			OnShow(argument);
        }
		public void Close()
        {
			animator.Play("WindowClose");
			StartCoroutine(IEClose());
        }

		private IEnumerator IEClose()
        {
			yield return new WaitForSeconds(0.5f);
			gameObject.SetActive(false);
        }

		protected abstract void OnShow(T argument);
	}
}