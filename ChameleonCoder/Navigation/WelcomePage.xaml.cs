using System;
using System.Windows.Controls;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a welcome page displaying several buttons
    /// </summary>
    internal sealed partial class WelcomePage : Page
    {
        /// <summary>
        /// creates a new instance of this page
        /// </summary>
        internal WelcomePage()
        {
            DataContext = App.Gui.DataContext;
            InitializeComponent();
        }

        /// <summary>
        /// switches view to the resource list
        /// </summary>
        /// <param name="sender">the control raising the event</param>
        /// <param name="e">additional data</param>
        private void OpenResourceList(object sender, EventArgs e)
        {
            App.Gui.GoList(null, null);
        }

        /// <summary>
        /// opens the 'NewResourceDialog'
        /// </summary>
        /// <param name="sender">the control raising the event</param>
        /// <param name="e">additional data</param>
        private void CreateResource(object sender, EventArgs e)
        {
            NewResourceDialog dialog = new NewResourceDialog();
            dialog.ShowDialog();
        }

        /// <summary>
        /// switches the view to the settings
        /// </summary>
        /// <param name="sender">the control raising the event</param>
        /// <param name="e">additional data</param>
        private void OpenConfiguration(object sender, EventArgs e)
        {
            App.Gui.GoSettings(null, null);
        }

        /// <summary>
        /// switches the view to the plugins view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenPlugins(object sender, EventArgs e)
        {
            App.Gui.GoPlugins(null, null);
        }
    }
}
