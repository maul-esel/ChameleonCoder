using System;
using System.Collections.Generic;

namespace ChameleonCoder.Resources.Interfaces
{
    /// <summary>
    /// an interface to implement by resources that are specific to a special coding langueg
    /// </summary>
    public interface ILanguageResource : IResource
    {
        /// <summary>
        /// the Identifier of the coding language's module
        /// </summary>
        Guid Language { get; }

        /// <summary>
        /// a list containing the identifiers of other coding languages the resource is compatible with
        /// </summary>
        IEnumerable<Guid> CompatibleLanguages { get; }
    }
}
