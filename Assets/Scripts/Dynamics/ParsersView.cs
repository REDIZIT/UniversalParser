using InGame.Dynamics.UI;
using System.Collections;
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
            StartCoroutine(IEBuildParser(model));
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

        private IEnumerator IEBuildParser(IParserModel model)
        {
            builder.Build(model);
            yield return null;
            ShowParser();
        }
    }
}