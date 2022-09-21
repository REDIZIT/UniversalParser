using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class ParserBuilder : MonoBehaviour
    {
        private DynamicParser parser;
        private DiContainer container;

        [Inject]
        private void Construct(DiContainer container)
        {
            this.container = container;
        }
        public void Build(IParserModel model)
        {
            parser = (DynamicParser)container.Instantiate(model.GetParserType());
            Debug.Log("Parser created: " + parser.GetType());
        }
        public void Clear()
        {
            parser.Stop();
        }
    }
}