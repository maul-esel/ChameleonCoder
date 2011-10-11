namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class RepresentationEventArgs : System.EventArgs
    {
        internal RepresentationEventArgs(ViewModelBase model, bool show)
        {
            Model = model;
            ShowRepresentation = show;
        }

        internal ViewModelBase Model
        {
            get;
            private set;
        }

        internal object Representation
        {
            get;
            set;
        }

        internal bool ShowRepresentation
        {
            get;
            private set;
        }
    }
}
