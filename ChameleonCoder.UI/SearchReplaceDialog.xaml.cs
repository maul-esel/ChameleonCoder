using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace ChameleonCoder.UI
{
    /// <summary>
    /// Interaktionslogik für SearchReplaceDialog.xaml
    /// </summary>
    [System.Runtime.InteropServices.ComVisible(false)]
    internal sealed partial class SearchReplaceDialog : Window
    {
        /// <summary>
        /// creates a new instance of the CCSearchDialog
        /// </summary>
        /// <param name="app">a reference to the app running the dialog</param>
        /// <param name="client">an interface instance for providing information and management of the text to be worked on</param>
        /// <param name="replace">true to begin in replace mode, false to begin in search mode</param>
        internal SearchReplaceDialog(IChameleonCoderApp app, ISearchReplaceClient client, bool replace)
        {
            DataContext = new ViewModel.SearchReplaceModel(app);
            InitializeComponent();

            this.client = client;

            enableReplace.IsChecked = replace;
        }

        private readonly ISearchReplaceClient client = null;

        private int currentPos = 0;

        private int currentLength = 0;

        /// <summary>
        /// reacts on a button click, searching for the next occurence of the string to search
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void SearchNext(object sender, EventArgs e)
        {
            SearchNext();
        }

        /// <summary>
        /// reacts on a button click, replacing the current selection with the replace text
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void Replace(object sender, EventArgs e)
        {
            client.ReplaceText(currentPos, currentLength, replaceBox.Text);
        }

        /// <summary>
        /// reacts on a button click, replacing all occurences of the search string with the replace string
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ReplaceAll(object sender, EventArgs e)
        {
            if (currentLength == 0)
                SearchNext();
            while (currentLength != 0)
            {
                client.ReplaceText(currentPos, currentLength, replaceBox.Text);
                SearchNext();
            }
        }

        /// <summary>
        /// searches for the next occurence of the search string and selects it
        /// </summary>
        private void SearchNext()
        {
            string text = client.GetText(); // save the text to search

            currentPos += currentLength; // update pos to avoid re-matching the same word
            currentLength = 0;

            if (!string.IsNullOrWhiteSpace(searchBox.Text) && !string.IsNullOrWhiteSpace(text)) // only search if both strings are non-empty
            {
                var pattern = searchBox.Text; // initialize the search pattern: the text to search for
                if (wholeOnly.IsChecked == true)
                {
                    pattern = "\\b" + pattern + "\\b"; // if whole word only: add word boundary signs
                }

                // create regex. If match case is checked, remove the "ignore" flag
                Regex regex = new Regex(pattern, matchCase.IsChecked == true ? RegexOptions.None : RegexOptions.IgnoreCase);

                // try to match the text to the pattern, starting from currentPos
                var match = regex.Match(text, currentPos);

                // if we did not find anything and we should wrap:
                if (match == null && wrapAround.IsChecked == true)
                    match = regex.Match(text, currentPos = 0); // try again, resetting currentPos

                if (match != null) // if we found sth.
                {
                    client.SelectText(match.Index, match.Length); // select it
                    currentLength = match.Length; // update the current position & length
                    currentPos = match.Index;
                }
            }
        }
    }
}
