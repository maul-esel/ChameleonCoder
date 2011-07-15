namespace ChameleonCoder.Resources.Interfaces
{
    public interface IAllowChildren
    {
        /// <summary>
        /// a ResourceCollection containing the (direct) child resources
        /// </summary>
        Collections.ResourceCollection children { get; }
    }
}
