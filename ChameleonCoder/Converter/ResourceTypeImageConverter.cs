using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(Type), typeof(ImageSource))]
    internal sealed class ResourceTypeImageConverter : IValueConverter
    {
        internal object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResourceTypeInfo info = ResourceTypeManager.GetInfo(value as Type);
            if (info != null)
                return info.TypeIcon.GetAsFrozen();
            return null;
        }

        internal object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
