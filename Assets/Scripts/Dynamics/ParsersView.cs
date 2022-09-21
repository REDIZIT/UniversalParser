using InGame.Dynamics.UI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class ParsersView : MonoBehaviour
    {
        [SerializeField] private ParserBuilder builder;
        [SerializeField] private Transform container;
        [SerializeField] private ParserModelUIItem prefab;

        private ParserModel[] parsers;

        [Inject]
        private void Construct(UIHelperPort uiHelper)
        {
            parsers = Resources.LoadAll<ParserModel>("Dynamics");

            uiHelper.FillContent(container, prefab, parsers, (item, model) => item.Refresh(model));
        }

        public void OnParserSelected(IParserModel model)
        {
            builder.Build(model);
        }
    }
}