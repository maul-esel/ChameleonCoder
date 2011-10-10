namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class TextSelectionEventArgs : System.EventArgs
    {
        internal TextSelectionEventArgs(int offset, int length)
        {
            StartOffset = offset;
            SelectionLength = length;
        }

        internal int StartOffset
        {
            get;
            private set;
        }

        internal int SelectionLength
        {
            get;
            private set;
        }
    }
}
