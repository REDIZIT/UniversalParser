using InGame.Parse;

namespace InGame.Dynamics
{
    public interface ISelectTable : IElement<ISelectTable.Model>
    {
        void SaveResult(IParseResult result);

        public class Model : ElementModel
        {

        }
    }
}