using System;
using UnityEngine;

namespace InGame.Dynamics
{
    [CreateAssetMenu(menuName = "SODB/Parsers/Cian districts")]
    public class CianDistrcitsModel : ParserModel
    {
        public override Type GetParserType()
        {
            return typeof(CianDistrictsParser);
        }
    }
}