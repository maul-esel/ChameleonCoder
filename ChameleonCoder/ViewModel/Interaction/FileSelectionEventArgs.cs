namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class FileSelectionEventArgs : System.EventArgs
    {
        internal FileSelectionEventArgs(string message)
            : this(message, System.Environment.CurrentDirectory, true)
        {
        }

        internal FileSelectionEventArgs(string message, bool mustExist)
            : this(message, System.Environment.CurrentDirectory, true)
        {
        }

        internal FileSelectionEventArgs(string message, string dir)
            : this(message, dir, true)
        {
        }

        internal FileSelectionEventArgs(string message, string dir, bool mustExist)
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
    }
}
