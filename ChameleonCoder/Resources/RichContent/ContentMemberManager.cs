using System;
using System.Collections.Concurrent;
using ChameleonCoder.Plugins;

namespace ChameleonCoder.Resources.RichContent
{
    public static class ContentMemberManager
    {
        internal static IContentMember CreateInstanceOf(string alias)
        {
            Type member = ContentMembers.GetMember(alias);
            if (member == null)
                return null;
            return Activator.CreateInstance(member) as IContentMember;
        }

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

        public static bool IsRegistered(string alias)
        {
            return ContentMembers.IsRegistered(alias);
        }

        public static bool IsRegistered(Type type)
        {
            return ContentMembers.IsRegistered(type);
        }

        public static IRichContentFactory GetFactory(Type component)
        {
            IRichContentFactory factory;
            if (Factories.TryGetValue(component, out factory))
                return factory;
            throw new ArgumentException("this is not a registered content member type", "component");
        }

        
    }
}
