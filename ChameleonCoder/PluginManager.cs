using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ChameleonCoder.Plugins
{
    internal delegate void PluginEvent(object sender, EventArgs e);
    
    /// <summary>
    /// manages all plugins
    /// </summary>
    internal sealed class PluginManager
    {
        internal static SortedList plugins { get; private set; }

        /// <summary>
        /// should call all classes to instantiate them.
        /// </summary>
        internal static void CallClasses()
        {
            // instantiate all classes in AHKScriptsMan.Plugins
            string[] files = Directory.GetFiles(Application.StartupPath + "\\Plugins", "*.dll");
            foreach (string file in files)
            {
                PluginInfo[] info = (PluginInfo[])Assembly.LoadFrom(file).EntryPoint.Invoke(null, null);
                foreach (PluginInfo lang in info)
                {
                    plugins.Add(lang.language, lang);
                }
            }

        }

        

    }
}
