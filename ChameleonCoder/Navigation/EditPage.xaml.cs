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
        /// <param name="model">the view model to use</param>
        internal EditPage(ViewModel.EditPageModel model)
        {
            model.SelectText += SelectText; 
            Initialize(model);
            InitializeComponent();
        }

        public ICSharpCode.AvalonEdit.TextEditor Editor
        {
            get { return editor; }
        }

        private void SelectText(object sender, ViewModel.Interaction.SelectionEventArgs e)
        {
            editor.Select(e.StartOffset, e.SelectionLength);
            var loc = editor.Document.GetLocation(e.StartOffset);
            editor.ScrollTo(loc.Line, loc.Column);
        }
    }
}
