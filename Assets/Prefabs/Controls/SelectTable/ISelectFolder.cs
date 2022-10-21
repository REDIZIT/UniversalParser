namespace InGame.Dynamics
{
    public interface ISelectFolder : IElement<ISelectFolder.Model>
    {
        string Path { get; }

        public class Model : IInputField.Model { }
    }
}