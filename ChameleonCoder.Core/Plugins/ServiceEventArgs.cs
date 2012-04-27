using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual)]
    public class ServiceEventArgs : System.EventArgs
    {
        internal ServiceEventArgs(IService service)
        {
            serviceInstance = service;
        }

        public IService Service
        {
            get { return serviceInstance; }
        }

        [ComVisible(false)]
        private readonly IService serviceInstance = null;
    }
}
