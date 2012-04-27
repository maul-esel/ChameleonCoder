using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.AutoDual)]
    public class ModuleEventArgs : System.EventArgs
    {
        internal ModuleEventArgs(ILanguageModule module)
        {
            moduleInstance = module;
        }

        public ILanguageModule Module
        {
            get { return moduleInstance; }
        }

        [ComVisible(false)]
        private readonly ILanguageModule moduleInstance = null;
    }
}
