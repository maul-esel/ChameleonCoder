using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für ResourceListPage.xaml
    /// </summary>
    public partial class ResourceListPage : Page
    {
        public ResourceListPage()
        {
            InitializeComponent();
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            IResource resource = e.Item as IResource;
            e.Accepted = true;

            if (resource is IResolvable && !App.Gui.ShowLinks.IsChecked == true)
            {
                e.Accepted = false;
            }
            else
            {
                // check if corresponding type is checked in ShowTypesList
            }
        }

        private void OpenResource(object sender, EventArgs e)
        {
            App.Gui.ResourceOpen(ResourceList.SelectedItem as IResource);
        }

        private void DroppedFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string file in files)
                {
                    System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                    try { doc.Load(file); }
                    catch (System.Xml.XmlException ex)
                    {
                        // check if it is a package file
                        MessageBox.Show(ex.Message + ex.Source);
                    }

                    App.AddResource(doc.DocumentElement, null);
                    System.IO.File.Copy(file,
                        System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)
                        + System.IO.Path.DirectorySeparatorChar + "Data" + System.IO.Path.DirectorySeparatorChar
                        + System.IO.Path.GetFileName(file));
                }
            }
        }

        internal void GroupingChanged(bool? enabled)
        {
            if (this.IsInitialized)
            {
                if (enabled == true)
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).GroupDescriptions.Add(new PropertyGroupDescription(null, new Converter.CustomGroupConverter()));
                else if (enabled == false)
                    CollectionViewSource.GetDefaultView(ResourceList.ItemsSource).GroupDescriptions.Clear();
            }
        }
    }
}
