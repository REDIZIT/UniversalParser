using UnityEngine;

namespace InGame.Dynamics
{
    public class LinkOpenner : MonoBehaviour
    {
        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}