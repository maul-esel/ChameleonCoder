using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual)]
    public class ResourceEventArgs : System.EventArgs
    {
        internal ResourceEventArgs(IResource resource)
        {
            resourceInstance = resource;
        }

        public IResource Resource
        {
            get { return resourceInstance; }
        }

        [ComVisible(false)]
        private readonly IResource resourceInstance = null;
    }
}
