using System;
using UnityEngine;

namespace InGame.Dynamics
{
    [CreateAssetMenu(menuName = "SODB/Parsers/ScreenMaker")]
    public class ScreenMakerModel : ParserModel
    {
        public override Type GetParserType()
        {
            return typeof(ScreenMakerDynamic);
        }
    }
}