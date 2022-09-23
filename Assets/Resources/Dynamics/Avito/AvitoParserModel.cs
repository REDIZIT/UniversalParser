using System;
using UnityEngine;

namespace InGame.Dynamics
{
    [CreateAssetMenu(menuName = "SODB/Parsers/Avito")]
    public class AvitoParserModel : ParserModel
    {
        public override Type GetParserType()
        {
            return typeof(AvitoDynamicParser);
        }
    }
}