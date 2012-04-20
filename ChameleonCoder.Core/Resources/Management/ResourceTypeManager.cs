using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Media;
using ChameleonCoder.Files;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Management
{
    /// <summary>
    /// manages the registered resource types
    /// </summary>
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
        private ResourceTypeCollection ResourceTypes = new ResourceTypeCollection();

        /// <summary>
        /// a dictionary associating the resource types with the registering component factory
        /// </summary>
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

        /// <summary>
        /// creates an instance of the type registered with the specified alias, using the given data
        /// </summary>
        /// <param name="key">the resource type key of the resource type</param>
        /// <param name="data">the XmlElement representing the resource</param>
        /// <param name="parent">the resource's parent</param>
        /// <returns>the new instance</returns>
        internal IResource CreateInstanceOf(Guid key, System.Xml.XmlElement data, IResource parent, DataFile file)
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
        /// creates an instance of the specified type,
        /// creates a new XmlElement representing the resource using the name and the specified attributes,
        /// and initializes the resource using the XmlElement and the given parent resource.
        /// </summary>
        /// <param name="type">the type to create an instance of</param>
        /// <param name="name">the name for the new resource</param>
        /// <param name="attributes">a list of attributes for the XmlElement</param>
        /// <param name="parent">the parent resource or null if a top-level resource is being created</param>
        /// <returns>the new resource</returns>
        public IResource CreateNewResource(Type type, string name, IDictionary<string, string> attributes, IResource parent, DataFile file)
        {
            var document = (parent == null) ? ChameleonCoderApp.DefaultFile.Document : parent.File.Document;
            var manager = XmlNamespaceManagerFactory.GetManager(document);

            var element = document.CreateElement("cc:resource", DataFile.NamespaceUri);
            element.SetAttribute("type", DataFile.NamespaceUri, GetKey(type).ToString("b"));

            if (parent == null)
                document.SelectSingleNode("/cc:ChameleonCoder/cc:resources", manager).AppendChild(element);
            else
                parent.Xml.AppendChild(element);

            foreach (var attribute in attributes)
            {
                element.SetAttribute(attribute.Key, DataFile.NamespaceUri, attribute.Value);
            }

            element.SetAttribute("name", DataFile.NamespaceUri, name);

            IResource resource = GetFactory(type).CreateInstance(type, element, parent, file);
            if (resource != null)
            {
                file.App.ResourceMan.Add(resource, parent);

                var data = ResourceHelper.GetDataElement(resource, true);
                var created = (System.Xml.XmlElement)document.CreateElement("cc:created", DataFile.NamespaceUri);
                created.InnerText = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                data.AppendChild(created);
            }

            return resource;
        }

        /// <summary>
        /// creates an instance of the type with the specified alias,
        /// creates a new XmlElement representing the resource using the name and the specified attributes,
        /// and initializes the resource using the XmlElement and the given parent resource.
        /// </summary>
        /// <param name="key">the resource type key of the type to create an instance of</param>
        /// <param name="name">the name for the new resource</param>
        /// <param name="attributes">a list of attributes for the XmlElement</param>
        /// <param name="parent">the parent resource or null if a top-level resource is being created</param>
        /// <returns>the new resource</returns>
        public IResource CreateNewResource(Guid key, string name, IDictionary<string, string> attributes, IResource parent, DataFile file)
        {
            return CreateNewResource(GetResourceType(key), name, attributes, parent, file);
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
