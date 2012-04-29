using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual)]
    public class PluginEventArgs : System.EventArgs
    {
        internal PluginEventArgs(IPlugin plugin)
        {
            pluginInstance = plugin;
        }

        public IPlugin Plugin
        {
            get { return pluginInstance; }
        }

        [ComVisible(false)]
        private readonly IPlugin pluginInstance = null;
    }
}
