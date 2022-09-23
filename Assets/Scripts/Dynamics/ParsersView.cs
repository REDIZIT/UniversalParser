using InGame.Dynamics.UI;
using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class ParsersView : MonoBehaviour
    {
        [SerializeField] private ParserBuilder builder;
        [SerializeField] private Transform container;
        [SerializeField] private ParserModelUIItem prefab;

        [SerializeField] private GameObject viewGroup, parserGroup;

        private ParserModel[] parsers;

        [Inject]
        private void Construct(UIHelperPort uiHelper)
        {
            parsers = Resources.LoadAll<ParserModel>("Dynamics");

            uiHelper.FillContent(container, prefab, parsers, (item, model) => item.Refresh(model, OnParserSelected));
        }
        private void Awake()
        {
            ShowView();
        }
        public void OnParserSelected(IParserModel model)
        {
            builder.Build(model);
            ShowParser();
        }

        public void ShowView()
        {
            viewGroup.SetActive(true);
            parserGroup.SetActive(false);
        }
        public void ShowParser()
        {
            viewGroup.SetActive(false);
            parserGroup.SetActive(true);
        }
    }
}