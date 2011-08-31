using System;
using System.Collections.Concurrent;
using System.Windows.Media;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Management
{
    public static class ResourceTypeManager
    {
        private static ResourceTypeCollection ResourceTypes = new ResourceTypeCollection();

        private static ConcurrentDictionary<Type, Plugins.IComponentFactory> Factories = new ConcurrentDictionary<Type, Plugins.IComponentFactory>();

        internal static Type GetResourceType(string alias)
        {
            return ResourceTypes.GetResourceType(alias);
        }

        internal static string GetAlias(Type type)
        {
            return ResourceTypes.GetAlias(type);
        }

        internal static IResource CreateInstanceOf(string alias)
        {
            Type type = GetResourceType(alias);
            if (type == null)
                return null;
            return Activator.CreateInstance(type) as IResource;
        }

        internal static System.Collections.Generic.IEnumerable<Type> GetResourceTypes()
        {
            return ResourceTypes.GetList();
        }

        internal static IComponentFactory GetFactory(Type component)
        {
            IComponentFactory factory;
            if (Factories.TryGetValue(component, out factory))
                return factory;
            throw new ArgumentException("this is not a registered resource type", "component");
        }

        public static string GetDisplayName(Type component)
        {
            return GetFactory(component).GetDisplayName(component);
        }

        public static ImageSource GetTypeIcon(Type component)
        {
            return GetFactory(component).GetTypeIcon(component);
        }

        public static Brush GetBackground(Type component)
        {
            return GetFactory(component).GetBackground(component);
        }

        public static bool IsRegistered(string alias)
        {
            return ResourceTypes.IsRegistered(alias);
        }

        public static bool IsRegistered(Type type)
        {
            return ResourceTypes.IsRegistered(type);
        }

        public static void RegisterComponent(Type component, string alias, Plugins.IComponentFactory factory)
        {
            if (component.GetInterface(typeof(IResource).FullName) != null
                && !component.IsAbstract && !component.IsInterface && !component.IsNotPublic
                && !IsRegistered(alias) && !IsRegistered(component)
                && !string.Equals(alias, "metadata", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(alias, "RichContent", StringComparison.OrdinalIgnoreCase))
            {
                ResourceTypes.RegisterResourceType(alias, component);
                Factories.TryAdd(component, factory);
            }
        }

        internal static void SetCollection(ResourceTypeCollection collection)
        {
            ResourceTypes = collection;
        }

        internal static ResourceTypeCollection GetCollection()
        {
            lock (ResourceTypes)
            {
                return ResourceTypes;
            }
        }
    }
}
