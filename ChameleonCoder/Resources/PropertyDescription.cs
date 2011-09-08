using System.ComponentModel;
using System.Reflection;

namespace ChameleonCoder.Resources
{
    /// <summary>
    /// encapsulates a resource's property for showing it to the resource view
    /// </summary>
    internal sealed class PropertyDescription : INotifyPropertyChanged
    {
        /// <summary>
        /// creates a new instance of the PropertyDescription class
        /// </summary>
        /// <param name="name">the property's display name</param>
        /// <param name="group">the property's display group</param>
        /// <param name="info">the PropertyInfo instance for the encapsulated property</param>
        /// <param name="instance">the instance the property belongs to</param>
        /// <param name="isReadOnly">a bool indicating whether the property should be read-only or not</param>
        internal PropertyDescription(string name, string group, PropertyInfo info, object instance, bool isReadOnly)
        {
            Name = name;
            Group = group;
            IsReadOnly = isReadOnly || info.GetSetMethod() == null;

            this.info = info;
            this.instance = instance;            

            (instance as INotifyPropertyChanged).PropertyChanged += OnPropertyChanged;
        }

        private object instance;
        
        private PropertyInfo info;

        public string Name { get; private set; }

        public string Group { get; private set; }

        public bool IsReadOnly { get; private set; }

        public object Value
        {
            get
            {
                var method = info.GetGetMethod(); // get the get-accessor
                if (method != null) // if found:
                    return method.Invoke(instance, null); // invoke it, using the instance as context
                return null; // if not found: return null
            }
            set
            {
                if (IsReadOnly) // if set to be read-only:
                    return; // stop
                var method = info.GetSetMethod(); // get the set-accessor
                if (method != null) // if found:
                    method.Invoke(instance, new object[1] { value }); // invoke it with the instance as context and the value as param
            }
        }

        /// <summary>
        /// raised when a property (the 'Value' property) changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// reacts to a change of the source property
        /// and raises the PropertyChanged event on 'Value'
        /// </summary>
        /// <param name="sender">the object raising the event</param>
        /// <param name="args">additional data related to the event</param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == info.Name)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }
    }
}
