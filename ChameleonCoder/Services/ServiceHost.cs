using System;
using System.Collections.Generic;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Services
{
    internal sealed class ServiceHost : IServiceHost
    {
        #region infrastructure

        private static Dictionary<Guid, IService> Services = new Dictionary<Guid, IService>();
        private static Guid ActiveService = Guid.Empty;
        private static ServiceHost instance = new ServiceHost();

        internal static void Add(Type type)
        {
            IService service = Activator.CreateInstance(type) as IService;
            if (service != null)
            {
                Services.Add(service.Service, service);
                service.Initialize(instance as IServiceHost);
            }
        }

        internal static void Shutdown()
        {
            foreach (IService service in Services.Values)
                service.Shutdown();
        }

        internal static void CallService(Guid ID)
        {
            IService service;
            if (Services.TryGetValue(ID, out service))
            {
                ActiveService = ID;
                service.Call();
            }
        }

        internal static int GetServiceCount()
        {
            return Services.Count;
        }

        internal static IEnumerable<IService> GetServices()
        {
            return Services.Values;
        }

        #endregion

        #region IServiceHost

        int IServiceHost.MinSupportedAPIVersion { get { return 1; } }

        int IServiceHost.MaxSupportedAPIVersion { get { return 1; } }

        #endregion

        #region IPluginHost

        void IPluginHost.AddMetadata(Guid resource, string name, string value)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddMetadata(Guid resource, string name, string value, bool isDefault, bool noconfig)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddResource(IResource resource)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddResource(IResource resource, Guid parent)
        {
            throw new NotImplementedException();
        }

        Guid IPluginHost.GetCurrentResource()
        {
            return ChameleonCoder.Resources.Management.ResourceManager.ActiveItem.GUID;
        }

        int IPluginHost.GetCurrentView()
        {
            throw new NotImplementedException();
        }

        IResource IPluginHost.GetResource(Guid ID)
        {
            return ChameleonCoder.Resources.Management.ResourceManager.FlatList.GetInstance(ID);
        }
        #endregion
    }
}
