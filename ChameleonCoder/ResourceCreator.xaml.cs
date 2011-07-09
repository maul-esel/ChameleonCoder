using System;
using System.Windows;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaktionslogik für ResourceCreator.xaml
    /// </summary>
    public partial class ResourceCreator : Window
    {
        public ResourceCreator()
        {
            InitializeComponent();

            this.LinkProperties.Visibility = System.Windows.Visibility.Collapsed;
            this.FileProperties.Visibility = System.Windows.Visibility.Collapsed;
            this.CodeProperties.Visibility = System.Windows.Visibility.Collapsed;
            this.LibraryProperties.Visibility = System.Windows.Visibility.Collapsed;
            this.ProjectProperties.Visibility = System.Windows.Visibility.Collapsed;
            this.TaskProperties.Visibility = System.Windows.Visibility.Collapsed;

            this.ResourceGUID.Text = Guid.NewGuid().ToString("B");

            this.Owner = App.Gui;
            this.ShowInTaskbar = false;
            this.ShowDialog();
        }

        private void SearchFile(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.InitialDirectory = ChameleonCoder.Properties.Settings.Default.ScriptDir;
            dialog.DereferenceLinks = false;

            dialog.FileOk += delegate(object sender2, System.ComponentModel.CancelEventArgs e2)
                {
                    if (!string.IsNullOrWhiteSpace((sender2 as System.Windows.Forms.OpenFileDialog).FileName))
                        this.ResourcePath.Text = (sender2 as System.Windows.Forms.OpenFileDialog).FileName;
                };


            dialog.ShowDialog();
        }

        private void TypeChanged(object sender, EventArgs e)
        {
            if (this.ResourceType.SelectedIndex == 0)
                this.LinkProperties.Visibility = System.Windows.Visibility.Visible;
            else
                this.LinkProperties.Visibility = System.Windows.Visibility.Collapsed;

            if (this.ResourceType.SelectedIndex > 0 && this.ResourceType.SelectedIndex < 4)
                this.FileProperties.Visibility = System.Windows.Visibility.Visible;
            else
                this.FileProperties.Visibility = System.Windows.Visibility.Collapsed;

            if (this.ResourceType.SelectedIndex > 1 && this.ResourceType.SelectedIndex < 4)
                this.CodeProperties.Visibility = System.Windows.Visibility.Visible;
            else
                this.CodeProperties.Visibility = System.Windows.Visibility.Collapsed;

            if (this.ResourceType.SelectedIndex == 3)
                this.LibraryProperties.Visibility = System.Windows.Visibility.Visible;
            else
                this.LibraryProperties.Visibility = System.Windows.Visibility.Collapsed;

            if (this.ResourceType.SelectedIndex == 4)
                this.ProjectProperties.Visibility = System.Windows.Visibility.Visible;
            else
                this.ProjectProperties.Visibility = System.Windows.Visibility.Collapsed;

            if (this.ResourceType.SelectedIndex == 5)
                this.TaskProperties.Visibility = System.Windows.Visibility.Visible;
            else
                this.TaskProperties.Visibility = System.Windows.Visibility.Collapsed;
        }

        private bool Validate()
        {
            if (this.ResourceType.SelectedIndex == -1)
                return false;

            if (string.IsNullOrWhiteSpace(this.ResourceName.Text))
            {
                this.NameError.Visibility = System.Windows.Visibility.Visible;
                return false;
            }
            else
                this.NameError.Visibility = System.Windows.Visibility.Collapsed;

            return true;
        }

        private void CreateResource(object sender, EventArgs e)
        {
            this.IsEnabled = false;
            if (this.Validate())
            {
                
            }
            this.IsEnabled = true;
        }
    }
}
