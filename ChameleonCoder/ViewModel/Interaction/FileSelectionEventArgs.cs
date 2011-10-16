namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class FileSelectionEventArgs : System.ComponentModel.CancelEventArgs
    {
        internal FileSelectionEventArgs(string message, string dir, string filter, bool mustExist)
        {
            Message = message;
            Directory = dir;
            MustExist = mustExist;
            Filter = filter;
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
