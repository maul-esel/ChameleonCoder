using System;
using System.Windows.Data;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Converter
{
    /// <summary>
    /// this converter returns the "static" (localized) DisplayName of an IResource implmentation, given a Type object
    /// </summary>
    [ValueConversion(typeof(Type), typeof(object))]
    internal sealed class ResourceTypeConverter : IValueConverter
    {
        /// <summary>
        /// converts the Type to the DisplayName
        /// </summary>
        /// <param name="value">the Type instance</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>the DisplayName</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // cast the value as Type
            Type type = value as Type;

            if (type != null) // if cast successful
            {
            	string property = parameter as string;
                if (property== "DisplayName")
                {
                    var displayName = ResourceTypeManager.GetDisplayName(type); // get the display name
                    if (!string.IsNullOrWhiteSpace(displayName)) // check if it is a blank
                        return displayName; // return it
                    // else throw an exception
                    throw new ArgumentException("this type's display name is blank: " + type.FullName, "value");
                }
                else if (property == "Icon")
                {
                    var icon = ResourceTypeManager.GetTypeIcon(type); // get the type icon
                    if (icon != null) // check if it is null
                        return icon; // return it
                    // else throw an exception
                    throw new ArgumentException("this type's type icon is null: " + type.FullName, "value");
                }
                throw new InvalidOperationException("no such information available: " + parameter);
            }
            // if 'value' is not a Type instance: throw exception
            throw new ArgumentException("'value' is either null or not a Type object", "value");
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
