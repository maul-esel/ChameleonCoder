using System;
using System.IO;

namespace ChameleonCoder.Shared
{
    /// <summary>
    /// a delegate for Settings events
    /// </summary>
    /// <param name="newValue">the setting's new value</param>
    [System.Runtime.InteropServices.ComVisible(false)]
    public delegate void SettingsEventHandler(object newValue);

    /// <summary>
    /// a public class providing information and notification for plugins
    /// </summary>
    [Obsolete, System.Runtime.InteropServices.ComVisible(false)]
    public static class InformationProvider
    {
        #region settings

        /// <summary>
        /// the user's Language as LCID code
        /// </summary>
        public static int Language { get { return Settings.ChameleonCoderSettings.Default.Language; } }

        #endregion

        #region information

        /// <summary>
        /// gets the type of page that is currently active
        /// </summary>
        public static CCTabPage CurrentPage
        {
            get
            {
                return CCTabPage.None; // ViewModel.MainWindowModel.Instance.ActiveTab.Type; // TODO!
            }
        }

        #endregion

        #region tools

#if ALL_STUFF
        /// <summary>
        /// registers a new CodeGenerator
        /// </summary>
        /// <param name="clicked">a delegate to invoke when the generator is invoked</param>
        /// <param name="image">the image to display for the CodeGenerator</param>
        /// <param name="text">the name of the CodeGenerator</param>
        public static void RegisterCodeGenerator(CodeGeneratorEventHandler clicked, ImageSource image, string text)
        {
            RibbonButton button = new RibbonButton() { Content = text, LargeImage = image };
            button.Click += (s, e) =>
                {
                    CodeGeneratorEventArgs args = new CodeGeneratorEventArgs();
                    clicked(CurrentResource, args);
                    if (!args.Handled)
                        InsertCode(args.Code);
                };
            ChameleonCoderApp.Gui.CustomGroup1.Controls.Add(button);
        }

        /// <summary>
        /// registers a new StubCreator
        /// </summary>
        public static void RegisterStubCreator()
        {
        } // IStubCreator

#endif

        #endregion

        #region Editing

#if ALL_STUFF
        /// <summary>
        /// appends code to the currently edited resource
        /// </summary>
        /// <param name="code">the code to insert</param>
        public static void AppendCode(string code)
        {
            var edit = ViewModel.MainWindowModel.Instance.ActiveTab.Content  as Navigation.EditPage;
            if (edit != null)
                edit.Editor.AppendText(code);
        }

        /// <summary>
        /// inserts code in the currently edited resource
        /// </summary>
        /// <param name="code">the code to insert</param>
        /// <param name="position">the position to use</param>
        public static void InsertCode(string code, int position)
        {
            var edit = ViewModel.MainWindowModel.Instance.ActiveTab.Content as Navigation.EditPage;
            if (edit != null)
                edit.Editor.Text = edit.Editor.Text.Insert(position, code);
        }

        /// <summary>
        /// inserts code in the currently edited resource at cursor position
        /// </summary>
        /// <param name="code">the code to insert</param>
        public static void InsertCode(string code)
        {
            var edit = ViewModel.MainWindowModel.Instance.ActiveTab.Content as Navigation.EditPage;
            if (edit != null)
                edit.Editor.Text = edit.Editor.Text.Insert(edit.Editor.CaretOffset, code);
        }

        /// <summary>
        /// gets the current edit control
        /// </summary>
        /// <returns>the edit control or null if no resource is currently edited</returns>
        public static ICSharpCode.AvalonEdit.TextEditor GetEditor()
        {
            if (ViewModel.MainWindowModel.Instance.ActiveTab != null)
            {
                var edit = ViewModel.MainWindowModel.Instance.ActiveTab.Content as Navigation.EditPage;
                if (edit != null)
                    return edit.Editor;
            }
            return null;
        }
#endif

        #endregion

        #region shared infrastructure

        /// <summary>
        /// finds a free path for a file or directory, given the directory and the base name.
        /// </summary>
        /// <param name="directory">the directory which should contain the file or directory</param>
        /// <param name="baseName">the base name for the new file or directory</param>
        /// <param name="isFile">true if the method should look for a free file path, false for a free directory path.</param>
        /// <returns></returns>
        public static string FindFreePath(string directory, string baseName, bool isFile)
        {
            directory = directory[directory.Length - 1] == Path.DirectorySeparatorChar
                ? directory : directory + Path.DirectorySeparatorChar;

            baseName = baseName.TrimStart(Path.DirectorySeparatorChar);

            string fileName = isFile
                ? Path.GetFileNameWithoutExtension(baseName) : baseName;

            string Extension = isFile
                ? Path.GetExtension(baseName) : string.Empty;

            string path = directory + fileName + Extension;
            int i = 0;

            while ((isFile ? File.Exists(path) : Directory.Exists(path)))
            {
                path = directory + fileName + "_" + i + Extension;
                i++;
            }

            return path;
        }

        #endregion

        #region events

        /// <summary>
        /// raised when the 'Language' setting changed
        /// </summary>
        public static event SettingsEventHandler LanguageChanged;

        /// <summary>
        /// raises the LanguageChanged event
        /// </summary>
        public static void OnLanguageChanged()
        {
            SettingsEventHandler handler = LanguageChanged;
            if (handler != null)
                handler(Language);
        }

        #endregion
    }
}
