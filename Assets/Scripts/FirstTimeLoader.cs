using UnityEngine;

namespace InGame
{
    public class FirstTimeLoader : MonoBehaviour
    {
		public static bool isLoadingFirstTime = true;


		void Start()
        {
			if (isLoadingFirstTime)
            {
				isLoadingFirstTime = false;

				Pathes.Initialize();
			}
        }
    }
}