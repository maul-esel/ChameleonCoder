using System;
using System.Windows;
using ChameleonCoder.Resources.Implementations;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder
{
    /// <summary>
    /// Interaktionslogik für ResourceCreator.xaml
    /// </summary>
    public partial class ResourceCreator : Window
    {
        Type target;
        System.Collections.Generic.Dictionary<string, Func<string>> customAttributes = new System.Collections.Generic.Dictionary<string, Func<string>>();

        public ResourceCreator(Type target)
        {
            InitializeComponent();

            this.ResourceGUID.Text = Guid.NewGuid().ToString("B");
            this.ResourceType.Text = ResourceTypeManager.GetInfo(target).DisplayName;

            if (target != typeof(LinkResource))
                _Destination.Visibility = ResourceDestination.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("destination", delegate() { return ResourceDestination.Text; });


            if (target != typeof(FileResource))
                _Path1.Visibility = _Path2.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("path", delegate() { return ResourcePath.Text; });

            if (target.GetInterface(typeof(ILanguageResource).FullName) == null)
                _Language.Visibility = ResourceLanguage.Visibility = Visibility.Collapsed;
            else
            {
                this.ResourceLanguage.ItemsSource = LanguageModules.LanguageModuleHost.GetList();
                customAttributes.Add("language", delegate() { return ResourceLanguage.Text; });
            }

            if (target.GetInterface(typeof(ICompilable).FullName) == null)
                _CompilePath1.Visibility = _CompilePath2.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("compilation-path", delegate() { return ResourceCompilePath.Text; });

            if (target != typeof(LibraryResource))
                _Author.Visibility = _Version.Visibility = _License.Visibility
                    = ResourceAuthor.Visibility = ResourceVersion.Visibility = ResourceLicense.Visibility
                    = Visibility.Hidden;
            else
            {
                customAttributes.Add("author", delegate() { return ResourceAuthor.Text; });
                customAttributes.Add("version", delegate() { return ResourceVersion.Text; });
                customAttributes.Add("license", delegate() { return ResourceLicense.Text; });
            }

            if (target != typeof(ProjectResource))
                _Priority.Visibility = ResourcePriority.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("priority", delegate
                {
                    return ((int)Enum.Parse(typeof(ProjectResource.ProjectPriority), ResourcePriority.Text, true)).ToString();
                });

            if (target != typeof(TaskResource))
                _Enddate.Visibility = ResourceEnddate.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("enddate", delegate() { return ResourceEnddate.Text; });

            this.target = target;

            this.Owner = App.Gui;
            this.ShowInTaskbar = false;
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

        private bool Validate()
        {
            return true;
        }

        private void CreateResource(object sender, EventArgs e)
        {
            this.IsEnabled = false;
            if (this.Validate())
            {
                //this.Close();
                this.DialogResult = true;
            }

            this.IsEnabled = true;
        }

        public System.Xml.XmlNode GetXmlNode()
        {
            string attributes = string.Empty;
            foreach (System.Collections.Generic.KeyValuePair<string, Func<string>> pair in customAttributes)
                attributes += " " + pair.Key + "=\"" + pair.Value() + "\"";

            string xml = "<{0} name=\"{1}\" guid=\"{2}\" description=\"{3}\" notes=\"\" {4}>\n</{0}>";
            xml = string.Format(xml, ResourceTypeManager.GetInfo(target).Alias,
                                        ResourceName.Text,
                                        ResourceGUID.Text,
                                        ResourceDescription.Text,
                                        attributes); // todo: custom attributes {4}

            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }
    }
}
