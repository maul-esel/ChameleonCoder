namespace ChameleonCoder.ViewModel.Interaction
{
    internal sealed class ViewChangedEventArgs : System.EventArgs
    {
        internal ViewChangedEventArgs(TabContext newView)
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
