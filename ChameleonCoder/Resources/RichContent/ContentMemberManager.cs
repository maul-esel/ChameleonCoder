using System;
using System.Collections.Concurrent;
using System.Xml;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.Resources.RichContent
{
    /// <summary>
    /// a class for managing the registered RichContent types
    /// </summary>
    public static class ContentMemberManager
    {
        /// <summary>
        /// the collection holding the RichContent types
        /// </summary>
        private static ContentMemberCollection ContentMembers = new ContentMemberCollection();

        /// <summary>
        /// a dictionary associating the resource types with the registering component factory
        /// </summary>
        private static ConcurrentDictionary<Type, IRichContentFactory> Factories = new ConcurrentDictionary<Type, IRichContentFactory>();

        /// <summary>
        /// registers a RichContent-type
        /// </summary>
        /// <param name="member">a Type object representing the registered class</param>
        /// <param name="alias">the Xml-Alias of the class</param>
        /// <param name="factory">the factory working with the class</param>
        public static void RegisterContentMember(Type member, string alias, IRichContentFactory factory)
        {
            if (member.GetInterface(typeof(IContentMember).FullName) != null
                && !member.IsAbstract && !member.IsInterface && !member.IsNotPublic // scope and type
                && member.GetConstructor(Type.EmptyTypes) != null // creation
                && !IsRegistered(alias) && !IsRegistered(member) // no double-registration
                && PluginManager.IsRichContentFactoryRegistered(factory)) // no anonymous registration
            {
                ContentMembers.RegisterContentMember(alias, member);
                Factories.TryAdd(member, factory);
            }
        }

        /// <summary>
        /// gets whether a RichContent type with the given alias is already registered or not
        /// </summary>
        /// <param name="alias">the alias to test</param>
        /// <returns>true if a RichContent type with this alias is already registered, false otherwise.</returns>
        public static bool IsRegistered(string alias)
        {
            return ContentMembers.IsRegistered(alias);
        }

        /// <summary>
        /// tests whether the given RichContent type is already registered or not
        /// </summary>
        /// <param name="type">the Type to test</param>
        /// <returns>true if the given Type is already registered, false otherwise.</returns>
        public static bool IsRegistered(Type type)
        {
            return ContentMembers.IsRegistered(type);
        }

        /// <summary>
        /// gets the <see cref="ChameleonCoder.Plugins.IRichContentFactory"/> that registered the given type.
        /// </summary>
        /// <param name="component">the type to get the factory for</param>
        /// <returns>the <see cref="ChameleonCoder.Plugins.IRichContentFactory"/> instance</returns>
        /// <exception cref="System.ArgumentException">thown if the given type is not registered.</exception>
        public static IRichContentFactory GetFactory(Type component)
        {
            IRichContentFactory factory;
            if (Factories.TryGetValue(component, out factory))
                return factory;
            throw new ArgumentException("this is not a registered content member type", "component");
        }

        internal static IContentMember CreateInstanceOf(string alias, XmlElement data, IContentMember parent)
        {
            Type member = ContentMembers.GetMember(alias);
            if (member != null)
            {
                var factory = GetFactory(member);
                if (factory != null)
                {
                    return factory.CreateInstance(member, data, parent);
                }
            }
            return null;
        }
        
    }
}
