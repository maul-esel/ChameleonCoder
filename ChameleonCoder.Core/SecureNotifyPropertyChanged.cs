using System.ComponentModel;
using System.Reflection;

namespace ChameleonCoder
{
    [System.Runtime.InteropServices.ComVisible(false)]
    internal abstract class SecureNotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string property)
        {
            if (GetType().GetProperty(property,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
                == null)
            {
                throw new System.InvalidOperationException("update unknown property: " + property);
            }

            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
