using System.Runtime.InteropServices;

namespace ChameleonCoder.Plugins
{
    [ComVisible(false), Guid("F75E21B2-A7F3-4E36-A6CD-C5064A1021D9"), ClassInterface(ClassInterfaceType.None)]
    public sealed class ModuleEventArgs : System.EventArgs, IModuleEventArgs
    {
        public ModuleEventArgs(ILanguageModule module)
        {
            moduleInstance = module;
        }

        public ILanguageModule Module
        {
            get { return moduleInstance; }
        }

        private readonly ILanguageModule moduleInstance = null;
    }
}
