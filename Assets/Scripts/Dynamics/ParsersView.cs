using System.Collections.Generic;
using UnityEngine;

namespace InGame.Dynamics
{
    public class ParsersView : MonoBehaviour
    {
        private ParserModel[] parsers;

        [SerializeField] private ParserBuilder builder;

        private void Start()
        {
            parsers = Resources.LoadAll<ParserModel>("Dynamics");

            //UIHelper.FillContent<>
        }

        public void OnParserSelected(IParserModel model)
        {
            builder.Build(model);
        }
    }
}