using System;
using System.Collections.Generic;
using System.Windows.Data;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Converter
{
    /// <summary>
    /// gets the properties of an IResource instance
    /// </summary>
    [ValueConversion(typeof(IResource), typeof(List<PropertyDescription>)), System.Runtime.InteropServices.ComVisible(false)]
    internal class ResourcePropertyConverter : IValueConverter
    {
        /// <summary>
        /// converts an IReasource instance into a list of PropertyDescriptions.
        /// </summary>
        /// <param name="value">the IResource instance</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>the list</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // check if the value param is non-null
            if (value != null)
            {
                // initialize the list and the Type instance
                List<PropertyDescription> list = new List<PropertyDescription>();
                Type type = value.GetType();

                // iterate through all public properties
                foreach (var property in type.GetProperties())
                {
                    // get the ResourcePropertyAttribute instance and the get-method
                    ResourcePropertyAttribute attr = (ResourcePropertyAttribute)Attribute.GetCustomAttribute(property, typeof(ResourcePropertyAttribute));
                    var getMethod = property.GetGetMethod();

                    // in case the ResourcePropertyAttribute is not defined on this property or (very rare) there's no public get-method...
                    if (attr == null || getMethod == null)
                        continue; // ...skip this property

                    // getting the group name
                    string group = string.Empty;
                    switch (attr.Group)
                    {
                        case ResourcePropertyGroup.General: // if it is defined as ResourcePropertyGroup.General
                            group = Properties.Resources.PropertyGroup_General; // use the localized group name
                            break;
                        case ResourcePropertyGroup.ThisClass: // if it is defined as ResourcePropertyGroup.ThisClass
                            if (!ChameleonCoderApp.RunningObject.ResourceTypeMan.IsRegistered(property.DeclaringType))
                                goto default;
                            group = ChameleonCoderApp.RunningObject.ResourceTypeMan.GetDisplayName(property.DeclaringType); // get the display name
                            break;
                        default:
                        case ResourcePropertyGroup.CurrentClass: // if it is defined as ResourcePropertyGroup.CurrentClass
                            group = ChameleonCoderApp.RunningObject.ResourceTypeMan.GetDisplayName(type); // use the display name
                            break; // checking is not needed: the current type must be registered
                    }
                    
                    // getting the property name
                    string name = attr.Name;
                    if (attr.IsReferenceName) // if it is the name of another property
                    {
                        var nameProperty = type.GetProperty(attr.Name, typeof(string)); // search for this property
                        if (nameProperty != null && nameProperty.GetGetMethod() != null) // if it is found and accessible...
                        {
                            string _name = (string)nameProperty.GetGetMethod().Invoke(value, null); // get it's value
                            if (!string.IsNullOrWhiteSpace(_name)) // check for empty names
                                name = _name; // and if valid, assign it
                        }
                    }
                    // add the PropertyDescription to the list
                    list.Add(new PropertyDescription(name, group, property, value, attr.IsReadOnly));
                }
                // return the list
                return list;
            }
            // if 'value' is null: throw an exception
            throw new ArgumentNullException("value");
        }

        /// <summary>
        /// this converter does not support converting back.
        /// </summary>
        /// <param name="value">not used</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
