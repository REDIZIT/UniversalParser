using System.Collections.Generic;
using UnityEngine;

namespace InGame.Dynamics.UI
{
    [CreateAssetMenu(menuName = "SODB/T")]
    public class Themes : ScriptableObject
    {
        [SerializeField] private ColorLayersDictionary colors;

        public Color GetColor(ColorLayer layer)
        {
            return colors[layer];
        }
    }
}