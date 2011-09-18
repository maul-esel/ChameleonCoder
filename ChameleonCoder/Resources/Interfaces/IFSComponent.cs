namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface to be implemented by resource types that correspond to a file system component
    /// </summary>
    public interface IFSComponent : IResource
    {
        /// <summary>
        /// returns the file system path
        /// </summary>
        /// <returns>the path</returns>
        /// <remarks>always use relative paths for storing and returning!</remarks>
        string GetFSPath();
    }
}
