using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ChameleonCoder.Plugins
{
    internal interface IManager
    {
        string GetUILanguage();
        // todo: include all functions PluginManager offers to plugins
        // problem: must be non-static
    }

    /// <summary>
    /// manages all plugins
    /// </summary>
    internal sealed class PluginManager : IManager
    {
        /// <summary>
        /// holds a list of all registered plugins
        /// </summary>
        private static SortedList plugins = new SortedList();

        /// <summary>
        /// the delegate type that is used for communication with plugins
        /// </summary>
        /// <param name="Language">the Guid of the language that is activated.
        /// This allows one method to handle several languages.</param>
        /// <param name="arguments">an event-specific collection of arguments</param>
        private delegate void PluginEvent(Guid Language, object[] arguments);

        /// <summary>
        /// the delegate type that is used for communication with plugins
        /// if a special resource is affected
        /// </summary>
        /// <param name="Language">the Guid of the language that is activated.
        /// This allows one method to handle several languages.</param>
        /// <param name="resource">the SortedList that contains the information</param>
        /// <param name="arguments">an event-specific collection of arguments</param>
        private delegate void ResourceEvent(Guid Language, SortedList resource, object[] arguments);

        /// <summary>
        /// defines the lexer type used for syntax highlighting
        /// </summary>
        private enum LexerType // evtl. include private in PluginManager?
        {
            /// <summary>
            /// no lexer is provided
            /// </summary>
            none,

            /// <summary>
            /// the lexer is included in the scintilla control
            /// </summary>
            interned,

            /// <summary>
            /// the lexer is located in an unmanaged DLL
            /// </summary>
            unmanaged,

            /// <summary>
            /// the lexer is included in the container app, in effect it has to be in the plugin
            /// </summary>
            container
        }

        private enum APIVersion
        {
            one
        }

        /// <summary>
        /// contains information about a language supported by a plugin
        /// </summary>
        private struct PluginInfo
        {
            /// <summary>
            /// the globally unique identifier of the language. It must always be the same!
            /// </summary>
            public Guid language;

            /// <summary>
            /// the language name which is displayed to the user
            /// </summary>
            public string languageName;

            /// <summary>
            /// the API version the plugin uses - may be useful for backwards compatibility in future versions
            /// </summary>
            public APIVersion API;

            /// <summary>
            /// called when the user switches to a resource in this language
            /// </summary>
            public PluginEvent LoadCallback;

            /// <summary>
            /// called when the user moves away form a resource in this language
            /// </summary>
            public PluginEvent UnloadCallback;

            /// <summary>
            /// called when the app closes
            /// </summary>
            public PluginEvent CloseCallback;

            /// <summary>
            /// called when the user attempts to compile a resource
            /// </summary>
            public ResourceEvent CompilerCallback;

            /// <summary>
            /// called when the user attempts to execute a resource
            /// </summary>
            public ResourceEvent ExecutionCallback;

            /// <summary>
            /// called when the user clicks the plugins about button
            /// </summary>
            public PluginEvent AboutBoxCallback;

            /// <summary>
            /// called when the user edits a resource in this language
            /// and thus enters a char in the edit control
            /// </summary>
            public PluginEvent CharTypedCallback;

            /// <summary>
            /// defines the lexer type used
            /// </summary>
            public LexerType Lexer;

            /// <summary>
            /// if the lexer is interned, this contains it number in the SCLEX enumeration
            /// </summary>
            public int InternedLexer;

            /// <summary>
            /// if the lexer is in an unmanaged DLL, this is the lexer's name
            /// to be loaded with SCI_LoadLexerLibrary.
            /// </summary>
            public string UnmanagedLexer;

            /// <summary>
            /// if the lexer is in an unmanaged DLL, this is the path to this dll.
            /// </summary>
            public string UnmanagedLexerDLL;

            /// <summary>
            /// if the lexer is contained in a plugin class, this is the callback to be called
            /// </summary>
            public PluginEvent StyleNeededCallback;

            // todo: add more delegates / events
        }

        /// <summary>
        /// loads all DLLs in the plugins folder and invokes their main method.
        /// </summary>
        internal static void LoadPlugins()
        {
            string[] files = Directory.GetFiles(Application.StartupPath + "\\Plugins", "*.dll");
            foreach (string file in files)
            {
                try
                {
                    // problem: dlls haben keinen EntryPoint
                    PluginInfo[] info = (PluginInfo[])Assembly.LoadFrom(file).EntryPoint.Invoke(null, null);
                    foreach (PluginInfo lang in info)
                    {
                        plugins.Add(lang.language, lang);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// calls all the close delegates to allow plugins to save unfinished work
        /// </summary>
        internal static void UnloadPlugins()
        {
            for (int i = 0; i <= plugins.Count; ++i)
            {
                ((PluginInfo)plugins.GetByIndex(i)).CloseCallback((Guid)plugins.GetKey(i), new string[] {});
            }
        }

        /// <summary>
        /// provides the name of a (coding) language
        /// </summary>
        /// <param name="language">the language's GUID</param>
        /// <returns>the language's name as string</returns>
        internal static string GetLanguageName(Guid language)
        {
            string result = string.Empty;
            try
            {
                result = ((PluginInfo)plugins.GetByIndex(plugins.IndexOfKey(language))).languageName;
            }
            catch { }
            return result;
        }

        /// <summary>
        /// provides the current UI language so that plugins can synchronize their localization
        /// </summary>
        /// <returns>a string like "en-GB" or "de-DE"</returns>
        string IManager.GetUILanguage()
        {
            return string.Empty;
        }
    }
}
