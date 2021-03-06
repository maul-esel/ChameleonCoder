﻿using System;
using System.Windows.Input;
using ChameleonCoder.Resources;
using ICSharpCode.AvalonEdit.Document;

namespace ChameleonCoder.UI.ViewModel
{
    /// <summary>
    /// a view model class for editing an IEditable resource
    /// </summary>
    [DefaultRepresentation(typeof(Navigation.EditPage))]
    internal sealed class EditPageModel : ViewModelBase, ISearchReplaceClient
    {
        /// <summary>
        /// creates a new instance for the given resource
        /// </summary>
        /// <param name="resource">the resource to edit</param>
        internal EditPageModel(IChameleonCoderApp app, IEditable resource)
            : base(app)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(resource.File.App == App, "Attempt to open file from another application instance was made!");
#endif

            App.ResourceMan.Open(resourceInstance = resource);

            Commands.Add(new CommandBinding(ChameleonCoderCommands.SaveResource,
                SaveResourceCommandExecuted));
            Commands.Add(new CommandBinding(NavigationCommands.Search,
                SearchCommandExecuted));
            Commands.Add(new CommandBinding(ApplicationCommands.Replace,
                ReplaceCommandExecuted));
            Commands.Add(new CommandBinding(NavigationCommands.DecreaseZoom,
                DecreaseZoomCommandExecuted));
            Commands.Add(new CommandBinding(NavigationCommands.IncreaseZoom,
                IncreaseZoomCommandExecuted));

            Settings.ChameleonCoderSettings.Default.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == "CodeFont")
                    {
                        OnPropertyChanged("CodeFont");
                    }
                };

            Document.Text = resource.GetText();
            Document.UndoStack.ClearAll();
        }

        #region commanding

        /// <summary>
        /// implements the logic for the ChameleonCoderCommands.SaveResource command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data related to execution</param>
        private void SaveResourceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (Resource.GetText() != Document.Text)
                Resource.SaveText(Document.Text);
        }

        /// <summary>
        /// implements the logic for the NavigationCommands.Search command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data related to execution</param>
        private void SearchCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SearchReplace(false);
        }

        /// <summary>
        /// implements the logic for the ApplicationCommands.Replace command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data related to execution</param>
        private void ReplaceCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SearchReplace(true);
        }

        /// <summary>
        /// implements the logic for the NavigationCommands.DecreaseZoom command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data related to execution</param>
        private void DecreaseZoomCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CodeFontSize -= 3;
        }

        /// <summary>
        /// implements the logic for the NavigationCommands.IncreaseZoom command
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">additional data related to execution</param>
        private void IncreaseZoomCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CodeFontSize += 3;
        }

        #endregion

        private void SearchReplace(bool replace)
        {
            var dialog = new SearchReplaceDialog(
                            App,
                            this,
                            replace);
            dialog.ShowDialog();
        }

        #region ISearchReplaceClient

        public string GetText()
        {
            return Document.Text;
        }

        public void ReplaceText(int offset, int length, string replaceText)
        {
            Document.Replace(offset, length, replaceText);
        }

        public void SelectText(int offset, int length)
        {
            OnSelectText(offset, length);
        }

        #endregion // "ISearchReplaceClient"

        /// <summary>
        /// gets the font size
        /// </summary>
        public int CodeFontSize
        {
            get { return fontSize; }
            private set
            {
                fontSize = value;
                OnPropertyChanged("CodeFontSize");
            }
        }

        private int fontSize = Settings.ChameleonCoderSettings.Default.CodeFontSize;

        /// <summary>
        /// gets the code font
        /// </summary>
        public System.Windows.Media.FontFamily CodeFont
        {
            get { return fontFamily; }
        }

        private System.Windows.Media.FontFamily fontFamily = new System.Windows.Media.FontFamily(Settings.ChameleonCoderSettings.Default.CodeFont);

        /// <summary>
        /// gets the highlighting definition
        /// </summary>
        public ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition Highlighting
        {
            get
            {
                ILanguageResource langRes = Resource as ILanguageResource;
                if (langRes != null && App.PluginMan.IsModuleRegistered(langRes.Language))
                {
                    return App.PluginMan.GetModule(langRes.Language).Highlighting;
                }
                return null;
            }
        }

        /// <summary>
        /// gets the document to display
        /// </summary>
        public TextDocument Document
        {
            get
            {
                return textDocument;
            }
        }

        private readonly TextDocument textDocument = new TextDocument();

        /// <summary>
        /// gets the resource being edited
        /// </summary>
        public IEditable Resource
        {
            get { return resourceInstance; }
        }

        private readonly IEditable resourceInstance = null;

        #region events

        internal event EventHandler<Interaction.TextSelectionEventArgs> SelectTextHandler;

        private void OnSelectText(int offset, int length)
        {
            var handler = SelectTextHandler;
            if (handler != null)
            {
                handler(this, new Interaction.TextSelectionEventArgs(offset, length));
            }
        }

        #endregion
    }
}
