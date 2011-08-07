using System;

namespace ChameleonCoder.LanguageModules
{
    /// <summary>
    /// defines the interface all language modules must implement
    /// </summary>
    public interface ILanguageModule : IPlugin
    {
        /// <summary>
        /// called when the user requests to compile a resource
        /// </summary>
        /// <param name="resource">the GUID of the resource to be compiled</param>
        void Compile(Resources.Interfaces.ICompilable resource);

        /// <summary>
        /// called when the user requests to execute a resource
        /// </summary>
        /// <param name="resource">the GUID of the resource to be executed</param>
        void Execute(Resources.Interfaces.IExecutable resource);

        /// <summary>
        /// called when the user opens a resource or switches to the editing panel
        /// </summary>
        void Load();

        /// <summary>
        /// called when the user leaves a resource or switches away from the editing panel
        /// </summary>
        void Unload();
    }
}
