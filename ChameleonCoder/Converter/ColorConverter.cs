using System;
using System.Windows.Data;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Converter
{
    internal sealed class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                ResourceTypeInfo info = Resources.Management.ResourceTypeManager.GetInfo(value.GetType());

                if (info != null && info.Background != null)
                    return info.Background.GetAsFrozen();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
