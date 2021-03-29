using UnityEngine;

namespace InGame.UI
{
	/// <summary>
	/// Created this class for more stable access to <see cref="TabsUI"/>
	/// </summary>
	public class BackButton : MonoBehaviour
	{
		public void GoBack()
        {
			GlobalUI.tabs.ShowAllParsers();
        }
	}
}