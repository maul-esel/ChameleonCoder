using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Windows.Media;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources.Management
{
    /// <summary>
    /// manages the registered resource types
    /// </summary>
    public static class ResourceTypeManager
    {
        /// <summary>
        /// the collection holding the resource types
        /// </summary>
        private static ResourceTypeCollection ResourceTypes = new ResourceTypeCollection();

        /// <summary>
        /// a dictionary associating the resource types with the registering component factory
        /// </summary>
        private static ConcurrentDictionary<Type, IResourceFactory> Factories = new ConcurrentDictionary<Type, IResourceFactory>();

        /// <summary>
        /// gets the resource type that was registered with the specified alias
        /// </summary>
        /// <param name="alias">the alias</param>
        /// <returns>the type</returns>
        internal static Type GetResourceType(string alias)
        {
            return ResourceTypes.GetResourceType(alias);
        }

        /// <summary>
        /// gets the alias a type was registered with
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the alias</returns>
        internal static string GetAlias(Type type)
        {
            return ResourceTypes.GetAlias(type);
        }

        /// <summary>
        /// creates an instance of the type registered with the specified alias
        /// </summary>
        /// <param name="alias">the alias</param>
        /// <returns>the instance</returns>
        [Obsolete("use overload", true)]
        internal static IResource CreateInstanceOf(string alias)
        {
            Type type = GetResourceType(alias);
            if (type == null)
                return null;
            return Activator.CreateInstance(type) as IResource;
        }

        /// <summary>
        /// creates an instance of the type registered with the specified alias, using the given data
        /// </summary>
        /// <param name="alias">the alias of the resource type</param>
        /// <param name="data">the XmlElement representing the resource</param>
        /// <param name="parent">the resource's parent</param>
        /// <returns>the new instance</returns>
        internal static IResource CreateInstanceOf(string alias, System.Xml.XmlElement data, IResource parent)
        {
            Type resourceType = GetResourceType(alias);
            if (resourceType != null)
            {
                var factory = GetFactory(resourceType);
                if (factory != null)
                    return factory.CreateInstance(resourceType, data, parent);
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
        public static IResource CreateNewResource(Type type, string name, IDictionary<string, string> attributes, IResource parent)
        {
            var resource = Activator.CreateInstance(type) as IResource;

            if (resource != null)
            {
                var document = resource.GetResourceFile().Document;

                var element = document.CreateElement(GetAlias(type));
                if (parent == null)
                    document.DocumentElement.SelectSingleNode("Resources").AppendChild(element);
                else
                    parent.Xml.AppendChild(element);

                foreach (var attribute in attributes)
                {
                    element.SetAttribute(attribute.Key, attribute.Value);
                }

                element.SetAttribute("name", name);

                resource.Initialize(element, parent);
                ResourceManager.Add(resource, parent);

                return resource;
            }

            return null;
        }

        /// <summary>
        /// creates an instance of the type with the specified alias,
        /// creates a new XmlElement representing the resource using the name and the specified attributes,
        /// and initializes the resource using the XmlElement and the given parent resource.
        /// </summary>
        /// <param name="alias">the alias of the type to create an instance of</param>
        /// <param name="name">the name for the new resource</param>
        /// <param name="attributes">a list of attributes for the XmlElement</param>
        /// <param name="parent">the parent resource or null if a top-level resource is being created</param>
        /// <returns>the new resource</returns>
        public static IResource CreateNewResource(string alias, string name, IDictionary<string, string> attributes, IResource parent)
        {
            return CreateNewResource(GetResourceType(alias), name, attributes, parent);
        }

        /// <summary>
        /// gets a list of all registered resource types
        /// </summary>
        /// <returns>the list</returns>
        internal static IEnumerable<Type> GetResourceTypes()
        {
            return ResourceTypes.GetList();
        }

        /// <summary>
        /// gets the factory that registered the given resource type
        /// </summary>
        /// <param name="component">the resource type</param>
        /// <returns>the IResourceFactory instance</returns>
        internal static IResourceFactory GetFactory(Type component)
        {
            IResourceFactory factory;
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

        public static void RegisterComponent(Type component, string alias, Plugins.IResourceFactory factory)
        {
            if (component.GetInterface(typeof(IResource).FullName) != null
                && !component.IsAbstract && !component.IsInterface && !component.IsNotPublic // scope and type
                && component.GetConstructor(Type.EmptyTypes) != null // creation
                && !IsRegistered(alias) && !IsRegistered(component) // no double-registration
                && PluginManager.IsResourceFactoryRegistered(factory)) // no anonymous registration
            {
                ResourceTypes.RegisterResourceType(alias, component);
                Factories.TryAdd(component, factory);
            }
        }
    }
}
