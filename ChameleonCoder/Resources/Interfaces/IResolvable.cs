namespace ChameleonCoder.Resources.Interfaces
{
    public interface IResolvable : IResource
    {
        IResource Resolve();

        bool shouldResolve { get; }
    }
}
