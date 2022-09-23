using InGame.UI;
using Zenject;

namespace InGame.Dynamics
{
    public class AvitoDynamicParser : DynamicParser
    {
        private InputFieldElement url;
        private PagingElement paging;
        private SelectTableElement table;

        [Inject]
        private void Construct(InputFieldElement url, PagingElement paging, SelectTableElement table, StatusElement status)
        {
            this.url = url;
            this.paging = paging;

            url.Setup(new InputFieldElement.Model()
            {
                labelText = "—сылка на авито с фильтрами",
                placeholderText = "—сылка"
            });
            paging.Setup(new PagingElement.Model()
            {

            });
            table.Setup(new SelectTableElement.Model());
            status.Setup(new StatusElement.Model(this)
            {

            });
        }
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }
    }
}