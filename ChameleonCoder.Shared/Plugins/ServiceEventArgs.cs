using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(false), Guid("CFEE4012-78EC-4BB5-B67D-20C3B5F9CA5A"), ClassInterface(ClassInterfaceType.None)]
    public sealed class ServiceEventArgs : System.EventArgs, IServiceEventArgs
    {
        public ServiceEventArgs(IService service)
        {
            serviceInstance = service;
        }

        public IService Service
        {
            get { return serviceInstance; }
        }

        private readonly IService serviceInstance = null;
    }
}
