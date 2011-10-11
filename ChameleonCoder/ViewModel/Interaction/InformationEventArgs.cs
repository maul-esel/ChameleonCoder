namespace ChameleonCoder.ViewModel.Interaction
{
    public sealed class InformationEventArgs<T> : System.EventArgs
    {
        internal InformationEventArgs() { }

        /// <summary>
        /// the information the sender receives
        /// </summary>
        public T Information
        {
            get;
            set;
        }
    }
}
