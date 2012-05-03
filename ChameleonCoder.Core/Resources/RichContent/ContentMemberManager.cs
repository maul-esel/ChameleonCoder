using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Xml;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.Resources.RichContent
{
    /// <summary>
    /// a class for managing the registered RichContent types
    /// </summary>
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual)]
    public sealed class ContentMemberManager
    {
        internal ContentMemberManager(ChameleonCoderApp app)
        {
            App = app;
        }

        public ChameleonCoderApp App
        {
            get;
            private set;
        }

        /// <summary>
        /// the collection holding the RichContent types
        /// </summary>
        [ComVisible(false)]
        private ContentMemberCollection ContentMembers = new ContentMemberCollection();

        /// <summary>
        /// a dictionary associating the resource types with the registering component factory
        /// </summary>
        [ComVisible(false)]
        private ConcurrentDictionary<Type, IRichContentFactory> Factories = new ConcurrentDictionary<Type, IRichContentFactory>();

        /// <summary>
        /// registers a RichContent-type
        /// </summary>
        /// <param name="member">a Type object representing the registered class</param>
        /// <param name="key">the resource type key of the class</param>
        /// <param name="factory">the factory working with the class</param>
        public void RegisterContentMember(Type member, Guid key, IRichContentFactory factory)
        {
            if (member.GetInterface(typeof(IContentMember).FullName) != null
                && !member.IsAbstract && !member.IsInterface && !member.IsNotPublic // scope and type
                && member.GetConstructor(Type.EmptyTypes) != null // creation
                && !IsRegistered(key) && !IsRegistered(member) // no double-registration
                && App.PluginMan.IsRichContentFactoryRegistered(factory.Identifier)) // no anonymous registration
            {
                ContentMembers.RegisterContentMember(key, member);
                Factories.TryAdd(member, factory);
            }
        }

        /// <summary>
        /// gets whether a RichContent type with the given alias is already registered or not
        /// </summary>
        /// <param name="key">the resource type key to test</param>
        /// <returns>true if a RichContent type with this key is already registered, false otherwise.</returns>
        public bool IsRegistered(Guid key)
        {
            return ContentMembers.IsRegistered(key);
        }

        /// <summary>
        /// tests whether the given RichContent type is already registered or not
        /// </summary>
        /// <param name="type">the Type to test</param>
        /// <returns>true if the given Type is already registered, false otherwise.</returns>
        public bool IsRegistered(Type type)
        {
            return ContentMembers.IsRegistered(type);
        }

        /// <summary>
        /// gets the <see cref="ChameleonCoder.Plugins.IRichContentFactory"/> that registered the given type.
        /// </summary>
        /// <param name="component">the type to get the factory for</param>
        /// <returns>the <see cref="ChameleonCoder.Plugins.IRichContentFactory"/> instance</returns>
        /// <exception cref="System.ArgumentException">thown if the given type is not registered.</exception>
        public IRichContentFactory GetFactory(Type component)
        {
            IRichContentFactory factory;
            if (Factories.TryGetValue(component, out factory))
                return factory;
            throw new ArgumentException("this is not a registered content member type", "component");
        }

        [Obsolete]
        internal IContentMember CreateInstanceOf(Guid key, XmlElement data, IContentMember parent)
        {
            var dict = new System.Collections.Specialized.ObservableStringDictionary();
            foreach (XmlAttribute attr in data.Attributes)
            {
                dict.Add(attr.Name, attr.Value);
            }
            return CreateInstanceOf(key, dict, parent, parent != null ? parent.File : null);
        }

        internal IContentMember CreateInstanceOf(Guid key, System.Collections.Specialized.ObservableStringDictionary data, IContentMember parent, Files.IDataFile file)
        {
            Type member = ContentMembers.GetMember(key);
            if (member != null)
            {
                var factory = GetFactory(member);
                if (factory != null)
                {
                    return factory.CreateInstance(member, data, parent, file);
                }
            }
            return null;
        }
    }
}
