using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// an interface to be implemented by resource types that correspond to a file system component
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("9f7b9512-bba6-4aa5-abc0-27dd9678fb5a")]
    public interface IFSComponent : IResource
    {
        /// <summary>
        /// returns the file system path
        /// </summary>
        /// <returns>the path</returns>
        /// <remarks>always use relative paths for storing and returning!</remarks>
        string GetFSPath(); // todo: possibly replace by GetStream() and IStreamableResource
    }
}
