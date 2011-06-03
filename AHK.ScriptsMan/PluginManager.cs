using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AHKScriptsMan.Plugins
{
    public delegate void PluginEvent(object sender, EventArgs e);
    
    /// <summary>
    /// manages all plugins
    /// </summary>
    public class PluginManager
    {
        public static SortedList plugins;

        /// <summary>
        /// should call all classes to instantiate them.
        /// </summary>
        public void CallClasses()
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
