using System.Windows.Controls;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying the edit control to edit resources
    /// </summary>
    internal sealed partial class EditPage : Page
    {
        /// <summary>
        /// creates a new instance of the page, given an IEditable resource
        /// </summary>
        /// <param name="resource">the resource to edit</param>
        internal EditPage(IEditable resource)
        {
            var model = new ViewModel.EditPageModel(resource);

            DataContext = model;
            CommandBindings.AddRange(model.Commands);

            InitializeComponent();
        }

        public ICSharpCode.AvalonEdit.TextEditor Editor
        {
            get { return editor; }
        }
    }
}
