using System;
using System.ComponentModel;

namespace ChameleonCoder.ViewModel
{
    /// <summary>
    /// an abstract base class for ViewModels
    /// </summary>
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
            Interaction.InformationProvider.LanguageChanged += (v) => UpdateAll();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal virtual void UpdateAll()
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                foreach (var property in GetType().GetProperties())
                {
                    var attr = (NotifyParentPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(NotifyParentPropertyAttribute));
                    if (attr == null || attr.NotifyParent == true)
                        handler(this, new PropertyChangedEventArgs(property.Name));
                }
            }
        }

        internal virtual void Update(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(property));
        }
    }
}
