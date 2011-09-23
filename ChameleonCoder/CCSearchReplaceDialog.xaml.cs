using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaktionslogik für CCSearchReplaceDialog.xaml
    /// </summary>
    internal sealed partial class CCSearchReplaceDialog : Window
    {
        /// <summary>
        /// creates a new instance of the CCSearchDialog
        /// </summary>
        /// <param name="getText">a delegate to get the current edit text</param>
        /// <param name="replaceText">a delegate to replace the specified area with the given text</param>
        /// <param name="selectText">a delegate to select the specified area</param>
        internal CCSearchReplaceDialog(Func<string> getText, Action<int, int, string> replaceText, Action<int, int> selectText, bool replace)
        {
            InitializeComponent();
            DataContext = new ViewModel.SearchReplaceModel();

            getTextDelegate = getText;
            replaceTextDelegate = replaceText;
            selectTextDelegate = selectText;

            enableReplace.IsChecked = replace;
        }

        private Func<string> getTextDelegate;

        private Action<int, int, string> replaceTextDelegate;

        private Action<int, int> selectTextDelegate;

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
            replaceTextDelegate(currentPos, currentLength, replaceBox.Text);
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
                replaceTextDelegate(currentPos, currentLength, replaceBox.Text);
                SearchNext();
            }
        }

        /// <summary>
        /// searches for the next occurence of the search string and selects it
        /// </summary>
        private void SearchNext()
        {
            string text = getTextDelegate(); // save the text to search

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
                    selectTextDelegate(match.Index, match.Length); // select it
                    currentLength = match.Length; // update the current position & length
                    currentPos = match.Index;
                }
            }
        }
    }
}
