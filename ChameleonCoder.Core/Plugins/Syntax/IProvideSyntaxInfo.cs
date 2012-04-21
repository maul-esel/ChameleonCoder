using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins.Syntax
{
    /// <summary>
    /// an interface classes can implement to provide information about a coding language's syntax.
    /// This can be used to enable interaction between IContentMembers, ILanguageResources and their ILanguageModules,
    /// for example to improve the syntax displayed by IContentMembers.
    /// </summary>
    /// <remarks>Although this is intended to be implemented by language modules, it does not inherit ILanguageModule
    /// and might be used in other places in the future.</remarks>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("4397c23f-bc72-48ad-9b36-bb2845f13eac")]
    public interface IProvideSyntaxInfo
    {
        /// <summary>
        /// gets if the coding language is case-sensitive.
        /// </summary>
        bool IsCaseSensitive { get; }

        /// <summary>
        /// tests whether the given feature is supported by the language
        /// </summary>
        /// <param name="element">the SyntaxElement to test for</param>
        /// <returns>true if the feature is supported, false if it is not supported.</returns>
        /// <remarks>As the SyntaxElement enum might grow over time,
        /// just return null if the handling of the given value is not implemented.</remarks>
        bool? IsSupported(SyntaxElement element);

        /// <summary>
        /// gets the syntax for the specified element
        /// </summary>
        /// <param name="element">The SyntaxElement to get the syntax for</param>
        /// <returns>An array of strings for possible syntax.</returns>
        /// <example>case SyntaxElement.ClassAccessor:
        ///     return new string[6] { "", "private", "internal", "protected", "protected internal", "public" };
        /// case SyntaxElement.Class:
        ///     return new string[1] { "class" };
        /// ...</example>
        /// <remarks>As the SyntaxElement enum might grow over time,
        /// just return null if the handling of the given value is not implemented.
        /// If the element is not supported by your language, throw an <see cref="System.NotSupportedException"/>.</remarks>
        string[] GetSyntax(SyntaxElement element);
    }
}
