using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Media;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// an interface used to manage components
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("69876e3c-bb40-4847-9d99-fd42508edffe")]
    public interface IResourceFactory : IPlugin
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
        /// provides information about a newly created resource,
        /// for example by letting the user modifiy its properties
        /// </summary>
        /// <param name="type">the type of the resource to create</param>
        /// <param name="name">the name for the resource</param>
        /// <param name="parent">the parent resource</param>
        /// <returns>a dictionary containing the attributes the resource's XML element should have</returns>
        /// <remarks>this function must not create an instance of the specified resource!</remarks>
        IDictionary<string, string> CreateResource(Type type, string name, IResource parent);

        /// <summary>
        /// gets a list of all types registered by this factory
        /// </summary>
        /// <returns>the Type-Array</returns>
        IEnumerable<Type> RegisteredTypes { get; }

        /// <summary>
        /// creates an instance of the given type
        /// </summary>
        /// <param name="resourceType">the type to create an instance of</param>
        /// <param name="data">the XmlElement representing the resource</param>
        /// <param name="parent">the parent resource</param>
        /// <returns>the newly created instance</returns>
        /// <remarks>This method should NOT create a new resource but instead instantiate an existing one.</remarks>
        IResource CreateInstance(Type resourceType, System.Xml.XmlElement data, IResource parent, Files.DataFile file);
    }
}
