namespace ChameleonCoder.UI.ViewModel.Interaction
{
    internal sealed class UserInputEventArgs : System.ComponentModel.CancelEventArgs
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
    }
}
