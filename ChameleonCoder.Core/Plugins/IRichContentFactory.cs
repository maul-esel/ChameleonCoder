using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// an interface to be implemented by factories for creation and management of RichContent members
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("924014d0-0788-43e7-acd7-2c9f3275e6f1")]
    public interface IRichContentFactory : IPlugin
    {
        /// <summary>
        /// gets a list of all types registered by this factory
        /// </summary>
        /// <returns>the Type-Array</returns>
        Type[] RegisteredTypes { get; }

        /// <summary>
        /// creates a new ContentMember of the given Type, using the given name and parent member
        /// </summary>
        /// <param name="type">the type of the member to create</param>
        /// <param name="name">the name of the new member</param>
        /// <param name="parent">the parent member or null</param>
        /// <returns>the newly created IContentMember instance</returns>
        [Obsolete("should return dictionary, same as in IResourceFactory", true)]
        IContentMember CreateMember(Type type, string name, IContentMember parent);

        ObservableStringDictionary CreateContentMember(Type type, string name, IContentMember parent);

        /// <summary>
        /// creates an instance of the given registered type
        /// </summary>
        /// <param name="memberType">the type to create an instance of</param>
        /// <param name="data">the XmlElement representing the member</param>
        /// <param name="parent">the parent member</param>
        /// <returns>the newly created instance</returns>
        [Obsolete]
        IContentMember CreateInstance(Type memberType, System.Xml.XmlElement data, IContentMember parent);

        IContentMember CreateInstance(Type memberType, ObservableStringDictionary data, IContentMember parent, Files.IDataFile file);
    }
}
