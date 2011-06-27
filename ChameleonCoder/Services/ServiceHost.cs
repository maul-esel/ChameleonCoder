using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace ChameleonCoder.Services
{
    internal sealed class ServiceHost : IServiceHost
    {
        #region infrastructure

        private static SortedList<Guid, IService> Services = new SortedList<Guid, IService>();
        private static Guid ActiveService = Guid.Empty;
        private static ServiceHost instance = new ServiceHost();

        internal static void Load()
        {
            // code from http://dotnet-snippets.de/dns/c-search-plugin-dlls-with-one-line-SID1089.aspx, slightly modified
            var result = from dll in Directory.GetFiles(Environment.CurrentDirectory + "\\Plugins", "*.dll")
                          let a = Assembly.LoadFrom(dll)
                          from t in a.GetTypes()
                          where t.GetInterface(typeof(IService).ToString()) != null
                          select Activator.CreateInstance(t) as IService;
            // ... up to here ;-)

            foreach (IService service in result)
            {
                if (service != null)
                {
                    Services.Add(service.Service, service);
                    service.Initialize(instance as IServiceHost);
                }
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

        void IPluginHost.AddResource(Resources.Base.ResourceBase resource)
        {
            throw new NotImplementedException();
        }

        void IPluginHost.AddResource(Resources.Base.ResourceBase resource, Guid parent)
        {
            throw new NotImplementedException();
        }

        Guid IPluginHost.GetCurrentResource()
        {
            return ChameleonCoder.Resources.Base.ResourceManager.ActiveItem.GUID;
        }

        int IPluginHost.GetCurrentView()
        {
            throw new NotImplementedException();
        }

        Resources.Base.ResourceBase IPluginHost.GetResource(Guid ID)
        {
            return ChameleonCoder.Resources.Base.ResourceManager.FlatList.GetInstance(ID);
        }
        #endregion
    }
}
