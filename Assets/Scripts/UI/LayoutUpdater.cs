using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
	[RequireComponent(typeof(LayoutGroup))]
	public class LayoutUpdater : MonoBehaviour
	{
		private LayoutGroup group;

        private void Awake()
        {
            group = GetComponent<LayoutGroup>();
        }

        private void Update()
        {
            group.enabled = false;
            group.enabled = true;
        }
    }
}