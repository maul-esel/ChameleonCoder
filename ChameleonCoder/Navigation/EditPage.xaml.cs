using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Navigation
{
    /// <summary>
    /// a page displaying the edit control to edit resources
    /// </summary>
    internal sealed partial class EditPage : CCPageBase
    {
        /// <summary>
        /// creates a new instance of the page, given an IEditable resource
        /// </summary>
        /// <param name="resource">the resource to edit</param>
        internal EditPage(IEditable resource)
        {
            Initialize(new ViewModel.EditPageModel(resource));
            InitializeComponent();
        }

        public ICSharpCode.AvalonEdit.TextEditor Editor
        {
            get { return editor; }
        }
    }
}
