using System.ComponentModel;

namespace ChameleonCoder
{
    internal abstract class SecureNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (GetType().GetProperty(property) == null)
                throw new System.InvalidOperationException("update unknown property: " + property);

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
