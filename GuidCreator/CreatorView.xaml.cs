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
            Invoke(null, null);
        }

        private void Invoke(object sender, EventArgs e)
        {
            UpdateDataContext(Guid.NewGuid());
            reportBox.Text = Properties.Resources.Report_Created;
        }

        private void UpdateDataContext(Guid guid)
        {
            DataContext = new
            {
                guid = (CurrentGuid = guid),
                action_create = Properties.Resources.Action_Create,
                action_enter = Properties.Resources.Action_Enter
            };
        }

        private void Enter(object sender, EventArgs e)
        {
            InputBox box = new InputBox("GUID Creator", Properties.Resources.Prompt_Enter,
                (s, error) =>
                {
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        error(Properties.Resources.Error_Empty);
                        return false;
                    }
                    Guid g;
                    if (!Guid.TryParse(s, out g))
                    {
                        error(Properties.Resources.Error_Invalid);
                        return false;
                    }
                    return true;
                });
            if (box.ShowDialog() == true)
            {
                UpdateDataContext(Guid.Parse(box.Text));
                reportBox.Text = Properties.Resources.Report_Entered;
            }
        }
    }
}
