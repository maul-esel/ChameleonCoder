using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Text;

namespace AHKScriptsMan.Plugins
{
    public delegate void NewPlugin(object sender, EventArgs e);
    
    /// <summary>
    /// manages all plugins
    /// (for future use)
    /// </summary>
    public class PluginManager
    {
        public static event NewPlugin PluginEvent;

        /// <summary>
        /// should call all classes to instantiate them.
        /// </summary>
        public void CallClasses()
        {
            // instantiate all classes in AHKScriptsMan.Plugins
            // 
            Type plugin = Assembly.Load("assemblyname").GetType("pluginname");
            plugin.FullName instance = Activator.CreateInstance(plugin);

        }

        public static void OnNewPlugin(EventArgs args)
        {
            if (PluginEvent != null)
            {
                PluginEvent(null, args);
            }
        }



    }
}
