using System;
using System.Windows.Data;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Converter
{
    /// <summary>
    /// this converter returns the 'Background'-brush from a ResourceTypeInfo instance
    /// </summary>
    [ValueConversion(typeof(IResource), typeof(System.Windows.Media.Brush))]
    internal sealed class ColorConverter : IValueConverter
    {
        /// <summary>
        /// returns the brush corresponding to the resource's type
        /// </summary>
        /// <param name="value">the IResource instance</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>the brush</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // check value to be null
            if (value != null)
            {
                // get the instance
                ResourceTypeInfo info = ResourceTypeManager.GetInfo(value.GetType());

                // check both the instance and the brush to be null (ComponentProviders could pass null)
                if (info != null && info.Background != null)
                    return info.Background.GetAsFrozen(); // use GetAsFrozen() to avoid multi-threading issues
                // if one of them is null: throw exception
                throw new NullReferenceException("the ResourceTypeInfo for " + value.GetType() + " or its 'Background' property is null.");
            }
            // if null: thro exception
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
            return null;
        }
    }
}
