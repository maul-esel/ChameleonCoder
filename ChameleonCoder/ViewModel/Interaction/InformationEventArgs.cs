namespace ChameleonCoder.ViewModel.Interaction
{
    /// <summary>
    /// a class that can be used to obtain typed information from a model client
    /// </summary>
    /// <typeparam name="T">the type of information to be obtained</typeparam>
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
