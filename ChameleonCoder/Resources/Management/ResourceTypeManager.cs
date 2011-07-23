using System;
using System.Collections.Generic;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Management
{
    public static class ResourceTypeManager
    {
        private static ResourceTypeCollection ResourceTypes = new ResourceTypeCollection();

        private static Dictionary<Type, ResourceTypeInfo> ResourceTypesStatic = new Dictionary<Type, ResourceTypeInfo>();

        public static Type GetResourceType(string alias)
        {
            return ResourceTypes.GetResourceType(alias);
        }

        internal static IResource CreateInstanceOf(string alias, params object[] parameters)
        {
            Type type = ResourceTypes.GetResourceType(alias);
            if (type == null)
                return null;
            return (IResource)Activator.CreateInstance(type, parameters);
        }

        public static IEnumerable<Type> GetResourceTypes()
        {
            return ResourceTypes.GetList();
        }

        public static ResourceTypeInfo GetInfo(Type t)
        {
            ResourceTypeInfo i;
            if (ResourceTypesStatic.TryGetValue(t, out i))
                return i;
            return null;
        }

        public static bool IsRegistered(string alias)
        {
            return ResourceTypes.IsRegistered(alias);
        }
        public static bool IsRegistered(Type resourceType)
        {
            return ResourceTypes.IsRegistered(resourceType);
        }

        internal static void RegisterComponent(Type component, ResourceTypeInfo info)
        {
            if (component.GetInterface(typeof(IResource).FullName) != null
                && !component.IsAbstract && !component.IsInterface && !component.IsNotPublic
                && !IsRegistered(info.Alias)
                && !IsRegistered(component)
                && !string.Equals(info.Alias, "metadata", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(info.Alias, "RichContent", StringComparison.OrdinalIgnoreCase))
            {
                ResourceTypes.RegisterResourceType(info.Alias, component);
                ResourceTypesStatic.Add(component, info);
            }
        }

        internal static void SetCollection(ResourceTypeCollection collection)
        {
            ResourceTypes = collection;
        }

        internal static ResourceTypeCollection GetCollection()
        {
            return ResourceTypes;
        }
    }
}
