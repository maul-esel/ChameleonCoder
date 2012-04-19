using System;
using System.Collections.Generic;
using System.Windows;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.ComponentCore.Resources
{
    /// <summary>
    /// A dialog for creating the resource types registered by ChameleonCoder.ComponentCore.
    /// </summary>
    internal sealed partial class ResourceCreator : Window
    {
        Dictionary<string, Func<string>> customAttributes = new Dictionary<string, Func<string>>();

        #region binding properties

        /// <summary>
        /// gets or sets the name of the resource
        /// </summary>
        /// <value>This property is given in the constructor.</value>
        public string ResName { get; private set; }

        /// <summary>
        /// gets or sets the identifier of the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        public Guid ResGuid { get; set; }

        /// <summary>
        /// gets or sets the description of the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        public string ResDescription { get; set; }

        /// <summary>
        /// gets or sets the path of the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        /// <remarks>This is intended for FileResources.</remarks>
        public string ResPath { get; set; }

        /// <summary>
        /// gets or sets the language module of the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        /// <remarks>This is intended for ILanguageResources.</remarks>
        public Plugins.ILanguageModule ResLanguage { get; set; }

        /// <summary>
        /// gets or sets the compilation path of the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        /// <remarks>This is intended for ICompilable resources.</remarks>
        public string ResCompilationPath { get; set; }

        /// <summary>
        /// gets or sets the author of the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        /// <remarks>This is intended for LibraryResources.</remarks>
        public string ResAuthor { get; set; }

        /// <summary>
        /// gets or sets the version of the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        /// <remarks>This is intended for LibraryResources.</remarks>
        public string ResVersion { get; set; }

        /// <summary>
        /// gets or sets the license of the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        /// <remarks>This is intended for LibraryResources.</remarks>
        public string ResLicense { get; set; }

        /// <summary>
        /// gets or sets the priority for the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        /// <remarks>This is intended for ProjectResources.</remarks>
        public ProjectPriority ResPriority { get; set; }

        /// <summary>
        /// gets or sets the enddate for the resource
        /// </summary>
        /// <value>This property is set through binding.</value>
        /// <remarks>This is intended for Taskresources.</remarks>
        public DateTime ResDate { get; set; }

        #endregion

        /// <summary>
        /// creates a new instance of the ResourceCreator dialog.
        /// </summary>
        /// <param name="target">The type to create a new resource of</param>
        /// <param name="parent">the name of the parent resource</param>
        /// <param name="name">the name of the new reosurce</param>
        public ResourceCreator(Type target, string parent, string name)
        {
            InitializeComponent();
            DataContext = this;

            ResName = name;
            ResourceParent.Text = parent;

            ResourceType.Text = ResourceTypeManager.GetDisplayName(target);

            ResGuid = Guid.NewGuid();

            if (target != typeof(FileResource))
                _Path1.Visibility = _Path2.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("path", () => ResPath);

            if (target.GetInterface(typeof(ILanguageResource).FullName) == null)
                _Language.Visibility = ResourceLanguage.Visibility = Visibility.Collapsed;
            else
            {
                this.ResourceLanguage.ItemsSource = ChameleonCoderApp.RunningObject.PluginMan.GetModules();
                customAttributes.Add("language", () => ResLanguage.Identifier.ToString("b"));
            }

            if (target != typeof(LibraryResource))
                _Author.Visibility = _Version.Visibility = _License.Visibility
                    = ResourceAuthor.Visibility = ResourceVersion.Visibility = ResourceLicense.Visibility
                    = Visibility.Hidden;
            else
            {
                customAttributes.Add("author", () => ResAuthor);
                customAttributes.Add("version", () => ResVersion);
                customAttributes.Add("license", () => ResLicense);
            }

            if (target != typeof(ProjectResource))
                _Priority.Visibility = ResourcePriority.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("priority",
                    () => ((int)ResPriority).ToString());

            if (target != typeof(TaskResource))
                _Enddate.Visibility = ResourceEnddate.Visibility = Visibility.Collapsed;
            else
                customAttributes.Add("enddate", () => ResDate.ToString());

            ShowInTaskbar = false;
        }

        /// <summary>
        /// shows a dialog and lets the user select the path of a resource
        /// </summary>
        /// <param name="sender">the button raising the event (not used)</param>
        /// <param name="e">additional data (not used)</param>
        private void SearchFile(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            dialog.InitialDirectory = Environment.CurrentDirectory;
            dialog.DereferenceLinks = false;

            dialog.FileOk += (s, e2) =>
                {
            		var file = (s as System.Windows.Forms.OpenFileDialog).FileName;
                    if (!string.IsNullOrWhiteSpace(file))
                        ResPath = file;
                };

            dialog.ShowDialog();
        }

        /// <summary>
        /// validates the entries in the dialog
        /// </summary>
        /// <returns>true (not yet implemented)</returns>
        private bool Validate()
        {
            return true;
        }

        /// <summary>
        /// if the entered data is valid, closes the dialog
        /// </summary>
        /// <param name="sender">the button raising the event (not used)</param>
        /// <param name="e">additional data (not used)</param>
        private void CreateResource(object sender, EventArgs e)
        {
            IsEnabled = false;

            if (Validate())
            {
                DialogResult = true;
                Close();
            }

            IsEnabled = true;
        }

        /// <summary>
        /// gets the resource's future attributes
        /// </summary>
        /// <returns>a Dictionary&lt;string, string&gt; instance containing the attributes</returns>
        public Dictionary<string, string> GetXmlAttributes()
        {
            var dict = new Dictionary<string, string>();

            dict.Add("name", ResName);
            dict.Add("id", ResGuid.ToString("b"));
            dict.Add("description", ResDescription);
            dict.Add("notes", string.Empty);

            foreach (var pair in customAttributes)
                dict.Add(pair.Key, pair.Value());

            return dict;
        }
    }
}
