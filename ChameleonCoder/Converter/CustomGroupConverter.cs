using System;
using System.Windows.Data;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Converter
{
    /// <summary>
    /// this converter converts an IResource instance into the corresponding Type.
    /// </summary>
    [ValueConversion(typeof(IResource), typeof(Type)), System.Runtime.InteropServices.ComVisible(false)]
    internal sealed class CustomGroupConverter : IValueConverter
    {
        /// <summary>
        /// converts the instance
        /// </summary>
        /// <param name="value">the IResource instance</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>the Type instance</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;

            // check if it is really an IResource instance
            if (resource != null)
            {
                return resource.GetType(); // ...return the type
            }
            // if it is not an instance or it was already null: throw an exception
            throw new ArgumentException("'value' is either null or not an instance of " + typeof(IResource).FullName + ".", "value");
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
