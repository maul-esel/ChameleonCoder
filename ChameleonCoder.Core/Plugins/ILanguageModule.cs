using System.Runtime.InteropServices;
using ChameleonCoder.Resources;
using ICSharpCode.AvalonEdit.Highlighting;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// defines the interface all language modules must implement
    /// </summary>
    [ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsDual), Guid("b2a1da1d-66fa-499e-9b47-d9b65d26ad6f")]
    public interface ILanguageModule : IPlugin
    {
        /// <summary>
        /// called when the user opens a resource or switches to the editing panel
        /// </summary>
        void Load();

        /// <summary>
        /// called when the user leaves a resource or switches away from the editing panel
        /// </summary>
        void Unload();

        /// <summary>
        /// checks if the language module can compile the specified resource
        /// </summary>
        /// <param name="resource">the resource to check</param>
        /// <returns>true if the resource can likely be compiled, false otherwise</returns>
        /// <remarks>For language resources, this is called on the specified language module. Otherwise, the user can choose from the available modules.</remarks>
        bool CanCompile(IResource resource);

        /// <summary>
        /// called when the user requests to compile a resource
        /// </summary>
        /// <param name="resource">the resource to be compiled</param>
        /// <remarks>This is only called if <see cref="CanCompile"/> returns true.
        /// Todo: possibly a return value indicating success or failure.</remarks>
        void Compile(IResource resource);

        /// <summary>
        /// checks if the language module can execute the specified resource
        /// </summary>
        /// <param name="resource">the resource to check</param>
        /// <returns>true if the resource can likely be executed, false otherwise</returns>
        /// <remarks>For language resources, this is called on the specified language module. Otherwise, the user can choose from the available modules.
        /// TODO: how to handle *.exe files?</remarks>
        bool CanExecute(IResource resource);

        /// <summary>
        /// called when the user requests to execute a resource
        /// </summary>
        /// <param name="resource">the resource to be executed</param>
        /// <remarks>This is only called if <see cref="CanExecute"/> returns true.
        /// Todo: possibly a return value indicating success or failure.</remarks>
        void Execute(IResource resource);

        /// <summary>
        /// the highlighting definition for the language, or null if none is available
        /// </summary>
        /// <remarks>You can set this property in many different ways:
        /// 1) simply ignore it and set it to null: no highlighting will be used
        /// 2) write a custom implementation of the IHighlightingDefinition class
        /// 3) load your definition from a *.xshd file using the <code>ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader</code> class
        /// 4) if your language is integrated in AvalonEdit, use <code>ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition(string)</code> or similar.</remarks>
        IHighlightingDefinition Highlighting { get; }
    }
}
