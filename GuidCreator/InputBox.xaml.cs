using System;
using System.Windows;

namespace GuidCreator
{
    /// <summary>
    /// Interaktionslogik für InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public Guid Guid { get; private set; }

        public InputBox()
        {
            InitializeComponent();
        }

        ChameleonCoder.Interaction.InvalidInputAdorner errorDisplay;

        public void Verify(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entered.Text))
            {
                errorDisplay = ChameleonCoder.Interaction.InvalidInputAdorner.Adorn(entered,
                    "enter a GUID to continue or click 'Cancel' to abort the operation");
                return;
            }
            else if (errorDisplay != null)
                errorDisplay.Remove();

            Guid g;
            if (!Guid.TryParse(entered.Text, out g))
            {
                errorDisplay = ChameleonCoder.Interaction.InvalidInputAdorner.Adorn(entered,
                    "please enter a valid GUID");
                return;
            }
            else if (errorDisplay != null)
                errorDisplay.Remove();

            Guid = g;
            DialogResult = true;
            Close();
        }
    }
}
