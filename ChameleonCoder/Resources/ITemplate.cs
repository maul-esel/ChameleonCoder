using System;
using System.Collections.Generic;
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
        /// The name of a template group the template should be added to.
        /// If the template should not be added to a group, use string.Empty
        /// </summary>
        string Group { get; }

        /// <summary>
        /// The Identifiers of the coding languages the template relies on,
        /// for example if it creates an ILanguageResource.
        /// If the template doesn't rely on a language, use null.
        /// </summary>
        List<Guid> Languages { get; }

        /// <summary>
        /// creates a new instance of the resource type and does additional operations
        /// </summary>
        /// <param name="parent">the parent resource or null is it is a new top resource</param>
        /// <returns>the newly created resource</returns>
        IResource Create(IResource parent);
    }
}
