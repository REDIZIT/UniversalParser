using InGame.Parse;
using static InGame.Dynamics.SelectTableElement;

namespace InGame.Dynamics
{
    public interface ISelectTable : IElement<ISelectTable.Model>
    {
        string FilePath { get; }

        void SaveResult(IParseResult result);

        public class Model : ElementModel
        {
            public TableSelectMode mode;
        }
    }
}