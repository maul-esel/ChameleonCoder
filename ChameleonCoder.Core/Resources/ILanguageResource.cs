using System;
using System.Runtime.InteropServices;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// an interface to implement by resources that are specific to a special coding langueg
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("83d09260-f106-4fff-b1e2-9bdc5743f8bb")]
    public interface ILanguageResource : IResource
    {
        /// <summary>
        /// the Identifier of the coding language's module
        /// </summary>
        Guid Language { get; }

        /// <summary>
        /// a list containing the identifiers of other coding languages the resource is compatible with
        /// </summary>
        Guid[] CompatibleLanguages { get; }
    }
}
