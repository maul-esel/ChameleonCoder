using System;
using System.Globalization;
using System.Windows.Data;

namespace ChameleonCoder.Converter
{
    /// <summary>
    /// this converter converts an integer into the corresponding culture name.
    /// </summary>
    [ValueConversion(typeof(int), typeof(string))]
    internal sealed class CultureConverter : IValueConverter
    {
        /// <summary>
        /// converts the LCID code into the native name of the corresponding culture.
        /// </summary>
        /// <param name="value">the LCID code</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>the CultureInfo.NativeName property</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new CultureInfo((int)value).NativeName;
        }

        /// <summary>
        /// this converter does not support converting back.
        /// </summary>
        /// <param name="value">not used</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
