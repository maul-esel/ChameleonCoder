namespace ChameleonCoder.UI.ViewModel.Interaction
{
    internal sealed class ReportEventArgs : System.EventArgs
    {
        internal ReportEventArgs(string topic, string message, MessageSeverity severity)
        {
            Topic = topic;
            Message = message;
            Severity = severity;
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

        internal MessageSeverity Severity
        {
            get;
            private set;
        }
    }
}
