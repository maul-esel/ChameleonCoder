namespace ChameleonCoder.Resources.Interfaces
{
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
