using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    public class FolderSelect : MonoBehaviour
    {
        public bool IsFilled { get; private set; }

        [SerializeField] private InputField pathField;

        private void Awake()
        {
            pathField.onValueChanged.AddListener(OnPathChanged);
        }
        private void OnDestroy()
        {
            pathField.onValueChanged.RemoveListener(OnPathChanged);
        }
        private void OnPathChanged(string s)
        {
            IsFilled = Directory.Exists(s);
        }
    }
}