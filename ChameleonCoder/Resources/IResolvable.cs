using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Resources
{
    public interface IResolvable
    {
        ResourceBase Resolve();

        bool shouldResolve { get; }
    }
}
