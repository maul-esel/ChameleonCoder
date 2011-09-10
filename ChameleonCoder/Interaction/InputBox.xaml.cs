using System;
using System.Windows;

namespace ChameleonCoder.Interaction
{
    /// <summary>
    /// Interaktionslogik für InputBox.xaml
    /// </summary>
    public sealed partial class InputBox : Window
    {
        public InputBox(string title, string prompt, Func<string, Action<string>, bool> validation)
        {
            InitializeComponent();
            Title = title;
            Prompt = prompt;
            _validation = validation;
        }

        public InputBox(string title, string prompt)
            : this(title, prompt, (s, e) => true)
        {
        }

        public string Prompt
        {
            get { return PromptText.Text; }
            set { PromptText.Text = value; }
        }

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

        ChameleonCoder.Interaction.InvalidInputAdorner errorDisplay;

        private void TextEntered(object sender, EventArgs e)
        {
            Verify();
        }

        public bool Verify()
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

        public void Close(object sender, EventArgs e)
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
