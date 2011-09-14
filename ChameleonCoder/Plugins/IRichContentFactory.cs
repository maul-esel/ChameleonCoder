using System;
using System.Collections.Generic;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.Plugins
{
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
        /// <returns>the newly created IContentMember instance</returns>
        IContentMember CreateMember(Type type, string name, IContentMember parent);
    }
}
