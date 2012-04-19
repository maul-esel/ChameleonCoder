using System;
using System.IO;
using System.Windows;

namespace ChameleonCoder
{
    /// <summary>
    /// a wrapper class to satisfy WPF
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    partial class App : Application
    {
        #region parameter constants

        /// <summary>
        /// param to install file extension
        /// </summary>
        private const string paramInstallExt = "--install_ext";

        /// <summary>
        /// param to uninstall file extension
        /// </summary>
        private const string paramUnInstallExt = "--uninstall_ext";

        /// <summary>
        /// param to install COM support
        /// </summary>
        private const string paramInstallCOM = "--install_COM";

        /// <summary>
        /// param to uninstall COM support
        /// </summary>
        private const string paramUnInstallCOM = "--uninstall_COM";

        #endregion

        /// <summary>
        /// executed on app startup, redirects to <see cref="ChameleonCoderApp.Open"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void InitHandler(object sender, StartupEventArgs e)
        {
            var obj = new ChameleonCoderApp();

            #region command line

            string path = null;
            int exitCode = 0;
            int argIndex = 0;

            // loop through all arguments
            foreach (string arg in e.Args)
            {
                if (argIndex == e.Args.Length - 1 && File.Exists(arg)) // the last argument is an existing file
                    path = arg;
                else // handle other parameters
                {
                    if (arg.Equals(paramInstallExt, StringComparison.OrdinalIgnoreCase))
                    {
                        obj.RegisterExtension();
                    }
                    else if (arg.Equals(paramUnInstallExt, StringComparison.OrdinalIgnoreCase))
                    {
                        obj.UnRegisterExtension();
                    }
                    else if (arg.Equals(paramInstallCOM, StringComparison.OrdinalIgnoreCase))
                    {
                        exitCode = -3; // not yet implemented
                    }
                    else if (arg.Equals(paramUnInstallCOM, StringComparison.OrdinalIgnoreCase))
                    {
                        exitCode = -3; // not yet implemented
                    }
                }
                argIndex++;
            }

            if (path == null) // no file to open was specified:
                obj.Exit(exitCode); // exit now (may this be changed to allow opening an empty app / new file?)

            #endregion

            var load = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                obj.LoadPlugins(); // load all plugins in the /Component/ folder
                obj.FileMan.Open(path); // open the file(s)
                obj.FileMan.LoadAll();
            });

            obj.InitWindow(); // create the window during plugin & file loading

            load.Wait(); // wait for plugins / files to be loaded
            obj.Show(); // show the GUI
        }
    }
}