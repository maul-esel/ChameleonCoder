using System;

namespace ChameleonCoder.LanguageModules
{
    internal sealed class LanguageModuleCollection : ComponentCollection<Guid, ILanguageModule>
    {
        internal void RegisterModule(ILanguageModule module)
        {
            base.RegisterComponent(module.Language, module);
        }

        internal ILanguageModule GetModule(Guid language)
        {
            return base.GetComponent(language);
        }
    }
}
