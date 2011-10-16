namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class ViewEventArgs : System.EventArgs
    {
        internal ViewEventArgs(TabContext newView)
        {
            newViewContext = newView;
        }

        internal TabContext NewView
        {
            get { return newViewContext; }
        }

        private readonly TabContext newViewContext;
    }
}
