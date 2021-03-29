using UnityEngine;
using UnityEngine.UI;

namespace InGame.UI
{
    [RequireComponent(typeof(CanvasScaler))]
	public class CanvasEditorSizer : MonoBehaviour
	{
        private void Awake()
        {
            CanvasScaler scaler = GetComponent<CanvasScaler>();

            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1300, 700) / (Application.isEditor ? 1 : 1.12f);
            //if (Application.isEditor)
            //{
            //    scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            //    scaler.referenceResolution = new Vector2(1300, 700);
            //}
            //else
            //{
            //    scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPhysicalSize;
            //    scaler.scaleFactor = 1.2f;
            //}
        }
    }
}