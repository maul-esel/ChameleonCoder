using System;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Plugins
{
    public interface ITemplate : IPlugin
    {
        /// <summary>
        /// the resource type the template creates
        /// </summary>
        Type ResourceType { get; }

        /// <summary>
        /// The name of a template group the template should be added to.
        /// If the template should not be added to a group, use string.Empty
        /// </summary>
        string Group { get; }

        /// <summary>
        /// creates a new instance of the resource type and does additional operations
        /// </summary>
        /// <param name="parent">the parent resource or null is it is a new top resource</param>
        /// <returns>the newly created resource</returns>
        IResource Create(IResource parent);
    }
}
