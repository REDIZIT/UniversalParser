namespace InGame.Dynamics
{
    public interface IElement<TModel> : IDynamicElement
    {
        void Setup(TModel model);
    }
    public class ElementModel
    {

    }
}