using Zenject;

namespace InGame.Dynamics
{
    public class AvitoDynamicParser : DynamicParser
    {
        private InputFieldElement url;
        private PagingElement paging;

        [Inject]
        private void Construct(InputFieldElement url, PagingElement paging, StatusElement status)
        {
            this.url = url;
            this.paging = paging;

            url.Setup(new InputFieldElement.Model()
            {
                labelText = "������ �� ����� � ���������",
                placeholderText = "������"
            });
            paging.Setup(new PagingElement.Model()
            {

            });
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