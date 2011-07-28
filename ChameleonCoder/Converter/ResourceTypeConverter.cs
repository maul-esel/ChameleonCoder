using System;
using System.Windows.Data;
using ChameleonCoder.Resources.Management;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(Type), typeof(string))]
    internal sealed class ResourceTypeConverter : IValueConverter
    {
        internal object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Type type = value as Type;

            if (type != null)
                return ResourceTypeManager.GetInfo(type).DisplayName;

            return null;
        }

        internal object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
