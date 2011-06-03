using System;

namespace AHKScriptsMan
{
    internal delegate void LanguageEvent(Guid language, object[] Arguments);
    internal struct PluginInfo
    {
        internal Guid language;
        internal string languageName;
        internal LanguageEvent CompilerCallback;
        internal LanguageEvent ExecutionCallback;
        internal LanguageEvent CharTypedCallback;
        //public LanguageEvent 
        // todo: add delegates / events
    }
}
