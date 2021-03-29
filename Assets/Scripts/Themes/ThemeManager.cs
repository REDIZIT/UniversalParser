using UnityEngine;

namespace InGame
{
	[ExecuteInEditMode]
	public class ThemeManager : MonoBehaviour
	{
		public static ThemeManager instance;

		[Header("Colors")]
		public Color background;
		public Color level1, level2, level3;

		public Color inputFieldColor;

		[Header("Images")]
		public Sprite inputFieldBackground;

        public ThemeManager()
        {
			instance = this;
        }
	}
}