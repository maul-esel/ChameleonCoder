using System;

namespace ChameleonCoder
{
    internal delegate void LanguageEvent(Guid language, object[] Arguments);
    internal struct PluginInfo
    {
        internal Guid language;
        internal string languageName;
        internal LanguageEvent LoadCallback;
        internal LanguageEvent MetadataReadCallback;
        internal LanguageEvent CompilerCallback;
        internal LanguageEvent ExecutionCallback;
        internal LanguageEvent CharTypedCallback;
        internal LanguageEvent AboutBoxCallback;
        internal LanguageEvent UnloadCallback;
        // todo: add delegates / events
    }
}
