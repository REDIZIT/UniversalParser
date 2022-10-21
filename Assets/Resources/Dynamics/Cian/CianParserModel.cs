using System;
using UnityEngine;

namespace InGame.Dynamics
{
    [CreateAssetMenu(menuName = "SODB/Parsers/CIAN")]
    public class CianParserModel : ParserModel
    {
        public override Type GetParserType()
        {
            return typeof(CianDynamicParser);
        }
    }
}