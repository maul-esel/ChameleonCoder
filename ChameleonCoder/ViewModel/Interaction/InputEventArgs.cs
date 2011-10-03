using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class UserInputEventArgs
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
