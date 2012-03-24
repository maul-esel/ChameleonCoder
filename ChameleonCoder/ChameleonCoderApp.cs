using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace ChameleonCoder
{
    /// <summary>
    /// The COM-visible new main class for the app
    /// </summary>
    [ComVisible(true), ProgId("ChameleonCoder.Application"), Guid("712fc748-468f-45db-ab09-e472b6a97b69"), ClassInterface(ClassInterfaceType.AutoDual)]
    public sealed class ChameleonCoderApp
    {
        /// <summary>
        /// registers the file extensions *.ccr
        /// </summary>
        /// <summary>
        /// the string used to separate resource paths
        /// </summary>
        [ComVisible(false)]
        internal const string resourcePathSeparator = "/";
        internal void RegisterExtension()
        {
            RegistryManager.RegisterExtension();
        }

        /// <summary>
        /// unregisters the file extensions *.ccr
        /// </summary>
        internal void UnRegisterExtension()
        {
            RegistryManager.UnRegisterExtension();
        }
        #region logging

        /// <summary>
        /// the path to the log file for the app
        /// </summary>
        [ComVisible(false)]
        internal static readonly string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ChameleonCoder.log");

        /// <summary>
        /// a template for the logging
        /// </summary>
        [ComVisible(false)]
        internal const string logTemplate = "new event:"
                                          + "\n\tsender: {0}"
                                          + "\n\treason: {1}"
                                          + "\n\t\t{2}"
                                          + "\n===========================================\n\n";

        /// <summary>
        /// logs a message, such as a warning, an error, ...
        /// </summary>
        /// <param name="sender">the sender calling this method</param>
        /// <param name="reason">the reason for the logging</param>
        /// <param name="text">more information about the event</param>
        [ComVisible(false)]
        public static void Log(string sender, string reason, string text)
        {
            if (!File.Exists(logPath)) // create file if necessary
                File.Create(logPath).Close(); // (and immediately close the stream)
            File.AppendAllText(logPath, string.Format(logTemplate, sender, reason, text)); // log
        }

        #endregion
    }
}
