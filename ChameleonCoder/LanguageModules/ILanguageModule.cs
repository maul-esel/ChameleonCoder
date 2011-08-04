using System;

namespace ChameleonCoder.LanguageModules
{
    /// <summary>
    /// defines the interface all language modules must implement
    /// </summary>
    public interface ILanguageModule
    {
        /// <summary>
        /// called when the app is closed, allows plugins to save unfinished work
        /// </summary>
        void Shutdown();

        /// <summary>
        /// the author of the Language Module
        /// </summary>
        string Author { get; }

        /// <summary>
        /// the custom module version
        /// </summary>
        string Version { get; }

        /// <summary>
        /// an about text displayed in an About Box. This may include credits, copyright, ...
        /// </summary>
        string About { get; }

        /// <summary>
        /// called when the app starts and loads all plugins
        /// </summary>
        /// <param name="Host">the instance of the ILanguageModuleHost interface the plugin can use to communicate.</param>
        void Initialize();

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

        /// <summary>
        /// the API version the plugin uses. Current is "1".
        /// </summary>
        int APIVersion { get; }

        /// <summary>
        /// the GUID that uniquely identifies the language. It should always be the same.
        /// Whenever it changes, all references to the language in any resources will break!
        /// </summary>
        Guid Language { get; }

        /// <summary>
        /// the name of the language, which is displayed to the user
        /// </summary>
        string LanguageName { get; }

        /// <summary>
        /// a image representing the service
        /// </summary>
        System.Windows.Media.ImageSource Icon { get; }

        /// <summary>
        /// defines whether the module is busy or not
        /// </summary>
        bool IsBusy { get; }
    }
}
