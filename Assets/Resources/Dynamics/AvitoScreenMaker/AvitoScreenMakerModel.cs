using System;
using UnityEngine;

namespace InGame.Dynamics
{
    [CreateAssetMenu(menuName = "SODB/Parsers/AvitoScreenMaker")]
    public class AvitoScreenMakerModel : ParserModel
    {
        public override Type GetParserType()
        {
            return typeof(AvitoScreenMakerDynamic);
        }
    }
}