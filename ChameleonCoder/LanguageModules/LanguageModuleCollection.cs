using System;

namespace ChameleonCoder.LanguageModules
{
    public sealed class LanguageModuleCollection : ComponentCollection<Guid, ILanguageModule>
    {
        public void RegisterModule(ILanguageModule module)
        {
            base.RegisterComponent(module.Language, module);
        }

        public ILanguageModule GetModule(Guid language)
        {
            return base.GetComponent(language);
        }
    }
}
