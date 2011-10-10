namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class FileSelectionEventArgs : System.EventArgs
    {
        internal FileSelectionEventArgs(string message, string dir, string filter, bool mustExist)
        {
            Message = message;
            Directory = dir;
            MustExist = mustExist;
        }

        internal string Message
        {
            get;
            private set;
        }

        internal string Directory
        {
            get;
            private set;
        }

        internal bool MustExist
        {
            get;
            private set;
        }

        internal string Path
        {
            get;
            set;
        }

        internal string Filter
        {
            get;
            private set;
        }
    }
}
