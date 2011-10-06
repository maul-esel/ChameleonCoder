namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class RepresentationEventArgs : System.EventArgs
    {
        internal RepresentationEventArgs(ViewModelBase model)
        {
            Model = model;
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
    }
}
