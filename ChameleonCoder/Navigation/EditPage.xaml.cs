using System.Windows.Controls;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying the edit control to edit resources
    /// </summary>
    public sealed partial class EditPage : Page
    {
        /// <summary>
        /// creates a new instance of the page, given an IEditable resource
        /// </summary>
        /// <param name="resource">the resource to edit</param>
        internal EditPage(IEditable resource)
        {
            InitializeComponent();

            ResourceManager.ActiveItem = Resource = resource;
            Editor.Text = resource.GetText();

            ILanguageResource langRes = resource as ILanguageResource;
            if (langRes != null)
            {
                if (PluginManager.ActiveModule != null)
                    PluginManager.UnloadModule();
                if (PluginManager.IsModuleRegistered(langRes.Language))
                    PluginManager.LoadModule(langRes.Language);
            }
        }

        /// <summary>
        /// the resource which is edited
        /// </summary>
        internal IEditable Resource { get; private set; }

        /// <summary>
        /// saves the changes to the resource
        /// </summary>
        internal void Save() // to be called from ribbon
        {
            Resource.SaveText(Editor.Text);
        }
    }
}
