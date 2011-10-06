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
        internal EditPage(ViewModel.EditPageModel model)
        {
            Initialize(model);
            InitializeComponent();
        }

        public ICSharpCode.AvalonEdit.TextEditor Editor
        {
            get { return editor; }
        }
    }
}
