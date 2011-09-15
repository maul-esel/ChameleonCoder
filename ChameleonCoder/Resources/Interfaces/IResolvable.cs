namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface to implement by resources that can be resolved to another resource
    /// </summary>
    public interface IResolvable : IResource
    {
        /// <summary>
        /// gets the instance the resource can be resolved to
        /// </summary>
        /// <returns>the IResource instance</returns>
        IResource Resolve();

        /// <summary>
        /// indicates whether to resolve the resource or not
        /// Do not use this.
        /// </summary>
        [System.Obsolete("need to find  abetter way, maybe a parameter. See github issue #16")]
        bool ShouldResolve { get; }
    }
}
