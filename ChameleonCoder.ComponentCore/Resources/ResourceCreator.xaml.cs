using System;
using System.Collections.Generic;
using System.Windows;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// Interaktionslogik für ResourceCreator.xaml
    /// </summary>
    public sealed partial class ResourceCreator : Window
    {
        Dictionary<string, Func<string>> customAttributes = new Dictionary<string, Func<string>>();

        public string ResName { get; set; }
        public Guid ResGuid { get; set; }
        public string ResDescription { get; set; }
        public Guid ResDestination { get; set; }
        public string ResPath { get; set; }
        public Plugins.ILanguageModule ResLanguage { get; set; }
        public string ResCompilationPath { get; set; }
        public string ResAuthor { get; set; }
        public string ResVersion { get; set; }
        public string ResLicense { get; set; }
        public ProjectPriority ResPriority { get; set; }
        public DateTime ResDate { get; set; }

        public ResourceCreator(Type target, string parent, string name)
        {
            InitializeComponent();
            this.DataContext = this;

            ResName = name;
            this.ResourceParent.Text = parent;

            this.ResourceType.Text = ResourceTypeManager.GetDisplayName(target);

            this.ResGuid = Guid.NewGuid();

            if (target != typeof(LinkResource))
                _Destination.Visibility = ResourceDestination.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("destination",() => ResourceDestination.Text);

            if (target != typeof(FileResource))
                _Path1.Visibility = _Path2.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("path", () => ResourcePath.Text);

            if (target.GetInterface(typeof(ILanguageResource).FullName) == null)
                _Language.Visibility = ResourceLanguage.Visibility = Visibility.Collapsed;
            else
            {
                this.ResourceLanguage.ItemsSource = Plugins.PluginManager.GetModules();
                customAttributes.Add("language", () => ResourceLanguage.Text);
            }

            if (target.GetInterface(typeof(ICompilable).FullName) == null)
                _CompilePath1.Visibility = _CompilePath2.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("compilation-path", () => ResourceCompilePath.Text);

            if (target != typeof(LibraryResource))
                _Author.Visibility = _Version.Visibility = _License.Visibility
                    = ResourceAuthor.Visibility = ResourceVersion.Visibility = ResourceLicense.Visibility
                    = Visibility.Hidden;
            else
            {
                customAttributes.Add("author", () => ResourceAuthor.Text);
                customAttributes.Add("version", () => ResourceVersion.Text);
                customAttributes.Add("license", () => ResourceLicense.Text);
            }

            if (target != typeof(ProjectResource))
                _Priority.Visibility = ResourcePriority.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("priority",
                    () => ((int)Enum.Parse(typeof(ProjectPriority), ResourcePriority.Text, true)).ToString());

            if (target != typeof(TaskResource))
                _Enddate.Visibility = ResourceEnddate.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("enddate", () => ResourceEnddate.Text);

            ShowInTaskbar = false;
        }

        private void SearchFile(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.InitialDirectory = DataFile.Directories[0];
            dialog.DereferenceLinks = false;

            dialog.FileOk += (s, e2) =>
                {
            		var file = (s as System.Windows.Forms.OpenFileDialog).FileName;
                    if (!string.IsNullOrWhiteSpace(file))
                        ResPath = file;
                };

            dialog.ShowDialog();
        }

        private bool Validate()
        {
            return true;
        }

        private void CreateResource(object sender, EventArgs e)
        {
            IsEnabled = false;
            if (Validate())
                DialogResult = true;

            IsEnabled = true;
        }

        public Dictionary<string, string> GetXmlAttributes()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("name", ResName);
            dict.Add("id", ResGuid.ToString("b"));
            dict.Add("description", ResDescription);
            dict.Add("notes", string.Empty);

            foreach (KeyValuePair<string, Func<string>> pair in customAttributes)
                dict.Add(pair.Key, pair.Value());

            return dict;
        }
    }
}
