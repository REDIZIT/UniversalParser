using System;
using UnityEngine;

namespace InGame.Dynamics
{
    [CreateAssetMenu(menuName = "SODB/Parsers/1000skladov")]
    public class ThousandSkladovModel : ParserModel
    {
        public override Type GetParserType()
        {
            return typeof(ThousandSkladovDynamicParser);
        }
    }
}