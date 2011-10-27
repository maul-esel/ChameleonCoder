using System;
using System.Collections.Generic;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// an interface to be implemented by factories for creation and management of RichContent members
    /// </summary>
    public interface IRichContentFactory : IPlugin
    {
        /// <summary>
        /// gets a list of all types registered by this factory
        /// </summary>
        /// <returns>the Type-Array</returns>
        IEnumerable<Type> RegisteredTypes { get; }

        /// <summary>
        /// creates a new ContentMember of the given Type, using the given name and parent member
        /// </summary>
        /// <param name="type">the type of the member to create</param>
        /// <param name="name">the name of the new member</param>
        /// <param name="parent">the parent member or null</param>
        /// <returns>a dictionary containing the attributes the new instance should have</returns>
        IDictionary<string, string> CreateMember(Type type, string name, IContentMember parent);

        /// <summary>
        /// creates an instance of the given registered type
        /// </summary>
        /// <param name="memberType">the type to create an instance of</param>
        /// <param name="data">the XmlElement representing the member</param>
        /// <param name="parent">the parent member</param>
        /// <returns>the newly created instance</returns>
        IContentMember CreateInstance(Type memberType, System.Xml.XmlElement data, IContentMember parent);
    }
}
