using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(false), Guid("FA5C142F-C75B-407D-81C6-6E2AB058617C"), ClassInterface(ClassInterfaceType.None)]
    public sealed class PluginEventArgs : System.EventArgs, IPluginEventArgs
    {
        public PluginEventArgs(IPlugin plugin)
        {
            pluginInstance = plugin;
        }

        public IPlugin Plugin
        {
            get { return pluginInstance; }
        }

        private readonly IPlugin pluginInstance = null;
    }
}
