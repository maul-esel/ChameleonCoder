using System;
using System.Windows.Data;

namespace ChameleonCoder.Services
{
    public class GuidConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Guid)value).ToString(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
