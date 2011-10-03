namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class ConfirmationEventArgs : System.EventArgs
    {
        internal ConfirmationEventArgs(string topic, string message)
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

        internal bool? Accepted
        {
            get;
            set;
        }
    }
}
