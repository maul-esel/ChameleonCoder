using System;
using System.Windows;

namespace ChameleonCoder.Shared
{
    /// <summary>
    /// A dialog to let the user enter some content
    /// </summary>
    public sealed partial class InputBox : Window
    {
        /// <summary>
        /// creates a new instance of the dialog
        /// </summary>
        /// <param name="title">the window title to use</param>
        /// <param name="prompt">the prompt to show to the user</param>
        /// <param name="validation">a delegate to validate the entered content</param>
        public InputBox(string title, string prompt, Func<string, Action<string>, bool> validation)
        {
            InitializeComponent();

            CancelBtn.Content = Properties.Resources.Action_Cancel;
            OKBtn.Content = Properties.Resources.Action_OK;

            Title = title;
            Prompt = prompt;
            _validation = validation;
        }

        /// <summary>
        /// creates a new instance of the dialog
        /// </summary>
        /// <param name="title">the window title to use</param>
        /// <param name="prompt">the prompt to show to the user</param>
        public InputBox(string title, string prompt)
            : this(title, prompt, (s, e) => true)
        {
        }

        /// <summary>
        /// gets or sets the prompt shown to the user
        /// </summary>
        public string Prompt
        {
            get { return PromptText.Text; }
            set { PromptText.Text = value; }
        }

        /// <summary>
        /// gets or sets the text entered by the user
        /// </summary>
        public string Text
        {
            get
            {
                if (IsInitialized && DialogResult == null)
                    return entered.Text;
                return _text;
            }
            set
            {
                if (IsInitialized && DialogResult == null)
                    entered.Text = value;
            }
        }

        private Func<string, Action<string>, bool> _validation;

        string _text;

        ChameleonCoder.Shared.InvalidInputAdorner errorDisplay;

        /// <summary>
        /// reacts to the TextBox' TextChanged event, validating the content
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void TextEntered(object sender, EventArgs e)
        {
            Verify();
        }

        /// <summary>
        /// verifies the entered content
        /// </summary>
        /// <returns>true if the content is valid, false otherwise</returns>
        private bool Verify()
        {
            if (_validation != null)
            {
                string error = null;
                if (!_validation(entered.Text, s => error = s))
                {
                    errorDisplay = InvalidInputAdorner.Adorn(entered, error);
                    return false;
                }
                else
                {
                    if (errorDisplay != null)
                        errorDisplay.Remove();
                    entered.ToolTip = null;
                    return true;
                }
            }
            return true;            
        }

        /// <summary>
        /// closes the box if the entered content is valid
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void Close(object sender, EventArgs e)
        {
            if (Verify())
            {
                _text = entered.Text;
                DialogResult = true;
                Close();
            }
        }
    }
}
