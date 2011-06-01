using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AHKScriptsMan.Plugins
{
    /// <summary>
    /// all plugins must implement this interface!
    /// </summary>
    public interface IPlugin
    {

        /// <summary>
        /// called when the application starts and the plugin system is loaded
        /// </summary>
        void LoadPlugin();

        /// <summary>
        /// called when the application is closed
        /// </summary>
        void UnloadPlugin();

        /// <summary>
        /// called whenever the plugin is told to be removed
        /// </summary>
        void UninstallPlugin();

        /// <summary>
        /// called when the plugin is added, before the first call to LoadPlugin()
        /// </summary>
        void InstallPlugin();

    }

    public interface ILexerPlugin : IPlugin
    {
        /// <summary>
        /// called when the control requests a style.
        /// </summary>
        void OnStyleNeeded();

    }


}
