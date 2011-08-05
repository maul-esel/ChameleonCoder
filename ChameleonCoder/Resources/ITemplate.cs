using System;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Resources
{
    public interface ITemplate : IComponent
    {
        /// <summary>
        /// the resource type the template creates
        /// </summary>
        Type ResourceType { get; }

        /// <summary>
        /// creates a new instance of the resource type and does additional operations
        /// </summary>
        /// <param name="parent">the parent resource or null is it is a new top resource</param>
        /// <returns>the newly created resource</returns>
        IResource Create(IResource parent);
    }
}
