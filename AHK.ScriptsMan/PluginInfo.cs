using System;

namespace AHKScriptsMan
{
    public delegate void LanguageEvent(Guid language, object[] Arguments);
    public struct PluginInfo
    {
        public Guid language;
        public string languageName;
        public LanguageEvent CompilerCallback;
        public LanguageEvent ExecutionCallback;
        public LanguageEvent CharTypedCallback;
        //public LanguageEvent 
        // todo: add delegates / events
    }
}
