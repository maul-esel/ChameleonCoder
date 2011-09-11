using ICSharpCode.AvalonEdit.Highlighting;

namespace ChameleonCoder.Plugins
{
    /// <summary>
    /// defines the interface all Language modules must implement
    /// </summary>
    public interface ILanguageModule : IPlugin
    {
        /// <summary>
        /// called when the user requests to compile a resource
        /// </summary>
        /// <param name="resource">the Identifier of the resource to be compiled</param>
        void Compile(Resources.Interfaces.ICompilable resource);

        /// <summary>
        /// called when the user requests to execute a resource
        /// </summary>
        /// <param name="resource">the Identifier of the resource to be executed</param>
        void Execute(Resources.Interfaces.IExecutable resource);

        /// <summary>
        /// called when the user opens a resource or switches to the editing panel
        /// </summary>
        void Load();

        /// <summary>
        /// called when the user leaves a resource or switches away from the editing panel
        /// </summary>
        void Unload();

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
