using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Resources.Management;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(Type), typeof(ImageSource))]
    sealed class ResourceTypeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ResourceTypeManager.GetInfo(value as Type).TypeIcon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
