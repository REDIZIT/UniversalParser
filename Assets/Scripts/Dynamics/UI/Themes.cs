using System.Collections.Generic;
using UnityEngine;

namespace InGame.Dynamics.UI
{
    [CreateAssetMenu(menuName = "SODB/Themes")]
    public class Themes : ScriptableObject
    {
        [SerializeField] private Dictionary<ColorLayer, Color> colors;

        public Color GetColor(ColorLayer layer)
        {
            return colors[layer];
        }
    }
}