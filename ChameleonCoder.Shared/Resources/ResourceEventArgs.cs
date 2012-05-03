using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources
{
    [ComVisible(false), Guid("CF965AE8-D1A8-47E7-B05B-096D56BB5BC4"), ClassInterface(ClassInterfaceType.None)]
    public class ResourceEventArgs : System.EventArgs, IResourceEventArgs
    {
        public ResourceEventArgs(IResource resource)
        {
            resourceInstance = resource;
        }

        public IResource Resource
        {
            get { return resourceInstance; }
        }

        private readonly IResource resourceInstance = null;
    }
}
