using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Microsoft.Windows.Controls.Ribbon;

namespace ChameleonCoder.Plugins
{
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
        private enum LexerType
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

        /// <summary>
        /// defines the API version (the version of IManager) a plugin uses
        /// </summary>
        private enum APIVersion
        {
            one
        }

        /// <summary>
        /// contains information about a language supported by a plugin
        /// </summary>
        private struct PluginInfo // todo: add more delegate types, add delegates for NewClass, NewFunction, ...
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
            [Obsolete]
            public LexerType Lexer;

            /// <summary>
            /// if the lexer is interned, this contains it number in the SCLEX enumeration
            /// </summary>
            [Obsolete]
            public int InternedLexer;

            /// <summary>
            /// if the lexer is in an unmanaged DLL, this is the lexer's name
            /// to be loaded with SCI_LoadLexerLibrary.
            /// </summary>
            [Obsolete]
            public string UnmanagedLexer;

            /// <summary>
            /// if the lexer is in an unmanaged DLL, this is the path to this dll.
            /// </summary>
            [Obsolete]
            public string UnmanagedLexerDLL;

            /// <summary>
            /// if the lexer is contained in a plugin class, this is the callback to be called
            /// </summary>
            [Obsolete]
            public PluginEvent StyleNeededCallback;

            // todo: add more delegates / events
        }

        /// <summary>
        /// loads all DLLs in the plugins folder and invokes their main method.
        /// </summary>
        internal void LoadPlugins()
        {
            if (Directory.Exists(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Plugins"))
            {
                string[] files = Directory.GetFiles(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\Plugins", "*.dll");
                foreach (string file in files)
                {
                    try
                    {
                        PluginInfo[] info = (PluginInfo[])Assembly.LoadFrom(file).GetTypes()[0].GetMethod("PluginMain").Invoke(null, new object[] { this });
                        foreach (PluginInfo lang in info)
                        {
                            plugins.Add(lang.language, lang);
                        }
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// calls all the close delegates to allow plugins to save unfinished work
        /// </summary>
        internal void UnloadPlugins()
        {
            for (int i = 0; i <= plugins.Count; i++)
            {
                ((PluginInfo)plugins.GetByIndex(i)).CloseCallback((Guid)plugins.GetKey(i), new string[] { });
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
        /// <returns>the CultureInfo object</returns>
        System.Globalization.CultureInfo IManager.GetUILanguage()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture;
        }

        /// <summary>
        /// adds a custom button to the ribbon, e.g. for code providers (UI designers, ...)
        /// </summary>
        /// <param name="Text">the text to display</param>
        /// <param name="image">the image to set</param>
        /// <param name="OnClick">the event handler for the click event</param>
        /// <param name="UIContext">an integer defining the ribbon tab</param>
        void IManager.AddButton(string Text, ImageSource image, RoutedEventHandler OnClick, int UIContext)
        {
            RibbonButton button = new RibbonButton();
            button.Label = Text;
            button.LargeImageSource = image;
            button.Click += OnClick;

            switch (UIContext)
            {
                case 0: App.Gui.CustomGroup1.Items.Add(button); break;
                case 1: App.Gui.CustomGroup2.Items.Add(button); break;
                case 2: App.Gui.CustomGroup3.Items.Add(button); break;
            }
        }

        /// <summary>
        /// inserts code into the current edit view
        /// </summary>
        /// <param name="code">the code to insert</param>
        void IManager.InsertCode(string code)
        {

        }

        /// <summary>
        /// adds metadata to a resource
        /// </summary>
        /// <param name="resource">the GUID of the resource to work on</param>
        /// <param name="name">the name of the metadata</param>
        /// <param name="value">the metadata's value</param>
        void IManager.AddMetadata(Guid resource, string name, string value)
        {

        }

    }
}
