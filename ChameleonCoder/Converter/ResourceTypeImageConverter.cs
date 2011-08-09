using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Converter
{
    /// <summary>
    /// converts a Type instance to a registered's resource type's icon.
    /// </summary>
    [ValueConversion(typeof(Type), typeof(ImageSource))]
    internal sealed class ResourceTypeImageConverter : IValueConverter
    {
        /// <summary>
        /// converts the Type instance to the ImageSource
        /// </summary>
        /// <param name="value">the Type instance to convert</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>the ImageSource</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Type type = value as Type; // cast the value to a Type instance
            if (type != null) // if cast successful
            {
                var image = ResourceTypeManager.GetTypeIcon(type); // get the icon
                if (image != null) // check if the type was registered with a null-icon
                    return image.GetAsFrozen(); // return the Icon, using GetAsFrozen() to avoid multi-threading issues
                // else throw exception
                throw new InvalidOperationException("the " + type.FullName + "'s Icon property is null");
            }
            // if cast not successful: throw exception
            throw new ArgumentException("'value' is not a Type instance or null", "value");
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
            return null;
        }
    }
}
