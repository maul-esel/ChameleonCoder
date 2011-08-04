using System;
using System.Collections.Concurrent;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Management
{
    internal static class ResourceTypeManager
    {
        private static ResourceTypeCollection ResourceTypes = new ResourceTypeCollection();

        private static ConcurrentDictionary<Type, ResourceTypeInfo> ResourceTypesStatic = new ConcurrentDictionary<Type, ResourceTypeInfo>();

        static object lock_gettype = new object();
        internal static Type GetResourceType(string alias)
        {
            lock (lock_gettype)
            {
                return ResourceTypes.GetResourceType(alias);
            }
        }

        static object lock_create = new object();
        internal static IResource CreateInstanceOf(string alias)
        {
            lock (lock_create)
            {
                Type type = ResourceTypes.GetResourceType(alias);
                if (type == null)
                    return null;
                return (IResource)Activator.CreateInstance(type);
            }
        }

        internal static System.Collections.Generic.IEnumerable<Type> GetResourceTypes()
        {
            return ResourceTypes.GetList();
        }

        static object lock_info = new object();
        internal static ResourceTypeInfo GetInfo(Type t)
        {
            lock (lock_info)
            {
                if (t != null)
                {
                    ResourceTypeInfo i;
                    if (ResourceTypesStatic.TryGetValue(t, out i))
                        return i;
                }
                return null;
            }
        }

        internal static bool IsRegistered(string alias)
        {
            return ResourceTypes.IsRegistered(alias);
        }

        internal static bool IsRegistered(Type type)
        {
            return ResourceTypes.IsRegistered(type);
        }

        internal static void RegisterComponent(Type component, ResourceTypeInfo info)
        {
            if (component.GetInterface(typeof(IResource).FullName) != null
                && !component.IsAbstract && !component.IsInterface && !component.IsNotPublic
                && !IsRegistered(info.Alias) && !IsRegistered(component)
                && !string.Equals(info.Alias, "metadata", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(info.Alias, "RichContent", StringComparison.OrdinalIgnoreCase))
            {
                ResourceTypes.RegisterResourceType(info.Alias, component);
                ResourceTypesStatic.TryAdd(component, info);
            }
        }

        internal static void SetCollection(ResourceTypeCollection collection)
        {
            ResourceTypes = collection;
        }

        static object lock_col = new object();
        internal static ResourceTypeCollection GetCollection()
        {
            lock (lock_col)
            {
                return ResourceTypes;
            }
        }
    }
}
