using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGame.Dynamics
{
    public class Scenes : MonoBehaviour
    {
        public void LoadOldInterface()
        {
            SceneManager.LoadScene("Main");
        }
        public void LoadNewInterface()
        {
            SceneManager.LoadScene("New");
        }
    }
}