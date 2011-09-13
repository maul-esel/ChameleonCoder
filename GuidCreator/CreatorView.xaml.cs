using System;
using System.Windows;
using ChameleonCoder.Interaction;

namespace ChameleonCoder.Services
{
    internal sealed partial class CreatorView : Window
    {
        internal Guid CurrentGuid { get; private set; }

        internal CreatorView()
        {
            InitializeComponent();
            DataContext = CurrentGuid = Guid.NewGuid();
        }

        internal void Invoke(object sender, EventArgs e)
        {
            DataContext = CurrentGuid = Guid.NewGuid();
        }

        internal void Enter(object sender, EventArgs e)
        {
            InputBox box = new InputBox("GuidCreator", "enter a GUID", (s, error) =>
                {
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        error("GUID must not be empty!");
                        return false;
                    }
                    Guid g;
                    if (!Guid.TryParse(s, out g))
                    {
                        error("You did not enter a valid GUID!");
                        return false;
                    }
                    return true;
                });
            if (box.ShowDialog() == true)
                DataContext = CurrentGuid = Guid.Parse(box.Text);
        }
    }
}
