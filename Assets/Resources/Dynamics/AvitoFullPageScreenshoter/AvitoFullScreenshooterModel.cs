using System;
using UnityEngine;

namespace InGame.Dynamics
{
    [CreateAssetMenu(menuName = "SODB/Parsers/AvitoFullScreenshoote")]
    public class AvitoFullScreenshooterModel : ParserModel
    {
        public override Type GetParserType()
        {
            return typeof(AvitoFullScreenshooter);
        }
    }
}