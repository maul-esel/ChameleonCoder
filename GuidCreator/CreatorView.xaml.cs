using System;
using System.Windows;
using ChameleonCoder.Interaction;

namespace GuidCreator
{
    public partial class CreatorView : Window
    {
        public Guid currentGuid { get; private set; }

        public CreatorView()
        {
            InitializeComponent();
            DataContext = currentGuid = Guid.NewGuid();
            this.ShowDialog();
        }

        public void Invoke(object sender, EventArgs e)
        {
            DataContext = currentGuid = Guid.NewGuid();
        }

        public void Enter(object sender, EventArgs e)
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
                DataContext = currentGuid = Guid.Parse(box.Text);
        }
    }
}
