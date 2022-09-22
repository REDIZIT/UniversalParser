using UnityEngine;
using Zenject;

namespace InGame.Dynamics
{
    public class ParserBuilder : MonoBehaviour
    {
        public DynamicParser ActiveParser { get; private set; }

        [SerializeField] private Transform[] containers;

        private DiContainer container;

        [Inject]
        private void Construct(DiContainer container)
        {
            this.container = container;
        }
        public void Build(IParserModel model)
        {
            foreach (Transform container in containers)
            {
                UIHelper.ClearChildren(container);
            }
            ActiveParser = (DynamicParser)container.Instantiate(model.GetParserType());
        }
        public void Clear()
        {
            ActiveParser?.Stop();
            ActiveParser = null;
        }
    }
}