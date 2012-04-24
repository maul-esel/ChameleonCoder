using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Windows.Media;
using ChameleonCoder.Files;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// manages the registered resource types
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual)]
    public sealed class ResourceTypeManager
    {
        internal ResourceTypeManager(ChameleonCoderApp app)
        {
            App = app;
        }

        public ChameleonCoderApp App
        {
            get;
            private set;
        }

        /// <summary>
        /// the collection holding the resource types
        /// </summary>
        [ComVisible(false)]
        private ResourceTypeCollection ResourceTypes = new ResourceTypeCollection();

        /// <summary>
        /// a dictionary associating the resource types with the registering component factory
        /// </summary>
        [ComVisible(false)]
        private ConcurrentDictionary<Type, IResourceFactory> Factories = new ConcurrentDictionary<Type, IResourceFactory>();

        /// <summary>
        /// gets the resource type that was registered with the specified alias
        /// </summary>
        /// <param name="key">resource type key</param>
        /// <returns>the type</returns>
        internal Type GetResourceType(Guid key)
        {
            return ResourceTypes.GetResourceType(key);
        }

        /// <summary>
        /// gets the alias a type was registered with
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the alias</returns>
        internal Guid GetKey(Type type)
        {
            return ResourceTypes.GetAlias(type);
        }

        internal IResource CreateInstanceOf(Guid key, ObservableStringDictionary data, IResource parent, IDataFile file)
        {
            Type resourceType = GetResourceType(key);
            if (resourceType != null)
            {
                var factory = GetFactory(resourceType);
                if (factory != null)
                    return factory.CreateInstance(resourceType, data, parent, file);
            }
            return null;
        }

        /// <summary>
        /// gets a list of all registered resource types
        /// </summary>
        /// <returns>the list</returns>
        internal IEnumerable<Type> GetResourceTypes()
        {
            return ResourceTypes.GetList();
        }

        /// <summary>
        /// gets the factory that registered the given resource type
        /// </summary>
        /// <param name="component">the resource type</param>
        /// <returns>the IResourceFactory instance</returns>
        public IResourceFactory GetFactory(Type component)
        {
            IResourceFactory factory;
            if (Factories.TryGetValue(component, out factory))
                return factory;
            throw new ArgumentException("this is not a registered resource type", "component");
        }

        /// <summary>
        /// gets the localized display name for the given resource type
        /// </summary>
        /// <param name="component">the reosurce type to get the name for</param>
        /// <returns>the localized string</returns>
        public string GetDisplayName(Type component)
        {
            return GetFactory(component).GetDisplayName(component);
        }

        /// <summary>
        /// gets the type icon for a given resource type
        /// </summary>
        /// <param name="component">the resource type to get the icon for</param>
        /// <returns>the icon as <see cref="System.Windows.Media.ImageSource"/> instance</returns>
        public ImageSource GetTypeIcon(Type component)
        {
            return GetFactory(component).GetTypeIcon(component);
        }

        /// <summary>
        /// gets the background brush for a given resource type
        /// </summary>
        /// <param name="component">the resource type to get the brush for</param>
        /// <returns>the <see cref="System.Windows.Media.Brush"/> instance</returns>
        public Brush GetBackground(Type component)
        {
            return GetFactory(component).GetBackground(component);
        }

        /// <summary>
        /// checks whether a resource type is registered with the given alias or not
        /// </summary>
        /// <param name="key">the resource type key of the type to check</param>
        /// <returns>true if a type with the given key is registered, false otherwise</returns>
        public bool IsRegistered(Guid key)
        {
            return ResourceTypes.IsRegistered(key);
        }

        /// <summary>
        /// checks if the given resource type is registered
        /// </summary>
        /// <param name="type">the resource type to check</param>
        /// <returns>true if the type is registered, false otherwise</returns>
        public bool IsRegistered(Type type)
        {
            return ResourceTypes.IsRegistered(type);
        }

        /// <summary>
        /// registers a new resource type
        /// </summary>
        /// <param name="component">the resource type to register</param>
        /// <param name="key">the resource type key to use for the type</param>
        /// <param name="factory">the <see cref="ChameleonCoder.Plugins.IResourceFactory"/> calling this method.</param>
        public void RegisterComponent(Type component, Guid key, Plugins.IResourceFactory factory)
        {
            if (component.GetInterface(typeof(IResource).FullName) != null
                && !component.IsAbstract && !component.IsInterface && !component.IsNotPublic // scope and type
                && !IsRegistered(key) && !IsRegistered(component) // no double-registration
                && App.PluginMan.IsResourceFactoryRegistered(factory)) // no anonymous registration
            {
                ResourceTypes.RegisterResourceType(key, component);
                Factories.TryAdd(component, factory);
            }
        }
    }
}
