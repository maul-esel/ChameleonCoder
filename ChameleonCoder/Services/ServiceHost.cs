using System;
using System.Collections.Generic;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Services
{
    internal static class ServiceHost
    {
        private static Dictionary<Guid, IService> Services = new Dictionary<Guid, IService>();
        private static Guid ActiveService = Guid.Empty;

        internal static void Add(Type type)
        {
            IService service = Activator.CreateInstance(type) as IService;
            if (service != null)
            {
                Services.Add(service.Identifier, service);
                service.Initialize();
            }
        }

        internal static void Shutdown()
        {
            foreach (IService service in Services.Values)
                service.Shutdown();
        }

        internal static int GetServiceCount()
        {
            return Services.Count;
        }

        internal static IEnumerable<IService> GetServices()
        {
            return Services.Values;
        }
    }
}
