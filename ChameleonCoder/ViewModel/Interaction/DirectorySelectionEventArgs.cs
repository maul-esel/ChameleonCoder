namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class DirectorySelectionEventArgs : System.ComponentModel.CancelEventArgs
    {
        internal DirectorySelectionEventArgs(string message, string dir, bool allowCreate)
        {
            Message = message;
            InitialDirectory = dir;
            AllowCreation = allowCreate;
        }

        internal string Message
        {
            get;
            private set;
        }

        internal string InitialDirectory
        {
            get;
            private set;
        }

        internal bool AllowCreation
        {
            get;
            private set;
        }

        internal string Path
        {
            get;
            set;
        }
    }
}
