using System;
using System.Windows.Media;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.RichContent;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// an interface used to manage components
    /// </summary>
    public interface IComponentFactory : IPlugin
    {
        /// <summary>
        /// gets the DisplayName for a resource- or RichContent-type
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the localized DisplayName</returns>
        string GetDisplayName(Type type);

        /// <summary>
        /// gets the TypeIcon for a resource- or RichContent-type
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the TypeIcon</returns>
        ImageSource GetTypeIcon(Type type);

        /// <summary>
        /// gets the background brush for a resource- or RichContent-type
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the brush</returns>
        Brush GetBackground(Type type);

        /// <summary>
        /// creates a new resource of the given Type, using the given name and parent resource
        /// </summary>
        /// <param name="type">the type of the resource to create</param>
        /// <param name="name">the name for the resource</param>
        /// <param name="parent">the parent resource or null if a top-level resource should be created</param>
        /// <returns>the newly created IResource instance</returns>
        IResource CreateResource(Type type, string name, IResource parent);

        /// <summary>
        /// creates a new ContentMember of the given Type, using the given name and parent member
        /// </summary>
        /// <param name="type">the type of the member to create</param>
        /// <param name="name">the name of the new member</param>
        /// <param name="parent">the parent member or null</param>
        /// <returns>the newly created IContentMember instance</returns>
        IContentMember CreateMember(Type type, string name, IContentMember parent);

        /// <summary>
        /// gets a list of all types registered by this factory
        /// </summary>
        /// <returns>the Type-Array</returns>
        Type[] GetRegisteredTypes();
    }
}
