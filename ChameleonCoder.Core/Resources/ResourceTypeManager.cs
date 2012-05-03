using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Media;
using ChameleonCoder.Files;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// manages the registered resource types
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    public sealed class ResourceTypeManager : IResourceTypeManager
    {
        internal ResourceTypeManager(IChameleonCoderApp app)
        {
            App = app;
        }

        public IChameleonCoderApp App
        {
            get;
            private set;
        }

        /// <summary>
        /// the collection holding the resource types
        /// </summary>
        private ResourceTypeCollection resourceTypeCollection = new ResourceTypeCollection();

        /// <summary>
        /// a dictionary associating the resource types with the registering component factory
        /// </summary>
        private ConcurrentDictionary<Type, IResourceFactory> Factories = new ConcurrentDictionary<Type, IResourceFactory>();

        /// <summary>
        /// gets the resource type that was registered with the specified alias
        /// </summary>
        /// <param name="key">resource type key</param>
        /// <returns>the type</returns>
        public Type GetResourceType(Guid key)
        {
            return resourceTypeCollection.GetResourceType(key);
        }

        /// <summary>
        /// gets the alias a type was registered with
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the alias</returns>
        public Guid GetKey(Type type)
        {
            return resourceTypeCollection.GetAlias(type);
        }

        public IResource CreateInstanceOf(Guid key, IObservableStringDictionary data, IResource parent, IDataFile file)
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
        public Type[] ResourceTypes
        {
            get
            {
                return new List<Type>(resourceTypeCollection.GetList()).ToArray();
            }
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
            return resourceTypeCollection.IsRegistered(key);
        }

        /// <summary>
        /// checks if the given resource type is registered
        /// </summary>
        /// <param name="type">the resource type to check</param>
        /// <returns>true if the type is registered, false otherwise</returns>
        public bool IsRegistered(Type type)
        {
            return resourceTypeCollection.IsRegistered(type);
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
                && App.PluginMan.IsResourceFactoryRegistered(factory.Identifier)) // no anonymous registration
            {
                resourceTypeCollection.RegisterResourceType(key, component);
                Factories.TryAdd(component, factory);
            }
        }

        public ITemplate GetDefaultTemplate(Type resourceType)
        {
            if (IsRegistered(resourceType))
                return new AutoTemplate(resourceType);
            return null;
        }
    }
}
