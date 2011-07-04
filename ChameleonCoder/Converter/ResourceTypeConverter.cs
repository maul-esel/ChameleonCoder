using System;
using System.Windows.Data;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(Type), typeof(string))]
    public sealed class ResourceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Type type = value as Type;

            if (type != null)
            {
                return (Activator.CreateInstance(type) as IResource).DisplayName;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
