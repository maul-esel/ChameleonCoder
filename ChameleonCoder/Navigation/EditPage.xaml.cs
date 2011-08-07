using System.Windows.Controls;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// Interaktionslogik für EditPage.xaml
    /// </summary>
    public partial class EditPage : Page
    {
        internal EditPage(IEditable resource)
        {
            InitializeComponent();
            this.Resource = resource;
            this.Editor.Text = resource.GetText();
            ILanguageResource langRes = resource as ILanguageResource;
            if (langRes != null)
            {
                if (PluginManager.ActiveModule != null)
                    PluginManager.UnloadModule();
                PluginManager.LoadModule(langRes.language);
            }
        }

        internal IEditable Resource { get; private set; }

        internal void Save() // to be called from ribbon
        {
            Resource.SaveText(Editor.Text);
        }
    }
}
