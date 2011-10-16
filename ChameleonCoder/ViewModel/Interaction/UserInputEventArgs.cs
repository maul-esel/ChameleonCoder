namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class UserInputEventArgs : System.EventArgs
    {
        internal UserInputEventArgs(string topic, string message)
        {
            Topic = topic;
            Message = message;
        }

        internal string Topic
        {
            get;
            private set;
        }

        internal string Message
        {
            get;
            private set;
        }

        internal string Input
        {
            get;
            set;
        }

        internal bool Canceled
        {
            get;
            set;
        }
    }
}
