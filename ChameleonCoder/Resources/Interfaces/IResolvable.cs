namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResolvable
    {
        IResource Resolve();

        bool shouldResolve { get; }
    }
}
