using System;

namespace ChameleonCoder
{
    public enum LexerType
    {
        none,
        interned,
        unmanaged,
        container
    }

    public delegate void LanguageEvent(Guid language, object[] Arguments);
    public struct PluginInfo
    {
        public Guid language;
        public string languageName;
        public LanguageEvent LoadCallback;
        public LanguageEvent MetadataReadCallback;
        public LanguageEvent CompilerCallback;
        public LanguageEvent ExecutionCallback;
        public LanguageEvent CharTypedCallback;
        public LexerType Lexer;
        public LanguageEvent AboutBoxCallback;
        public LanguageEvent UnloadCallback;

        
        public int InternedLexer;
        public string[] UnmanagedLexer;
        public LanguageEvent StyleNeededCallback;
        // todo: add more delegates / events
    }
}
