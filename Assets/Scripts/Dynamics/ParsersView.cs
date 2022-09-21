using System.Collections.Generic;
using UnityEngine;

namespace InGame.Dynamics
{
    public class ParsersView : MonoBehaviour
    {
        public List<ParserModel> parsers = new List<ParserModel>();

        [SerializeField] private ParserBuilder builder;

        private void Start()
        {
            OnParserSelected(parsers[0]);
        }

        public void OnParserSelected(IParserModel model)
        {
            builder.Build(model);
        }
    }
}