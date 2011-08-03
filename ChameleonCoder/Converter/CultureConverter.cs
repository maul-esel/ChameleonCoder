using System;
using System.Windows.Data;
using System.Globalization;

namespace ChameleonCoder.Converter
{
    internal sealed class CultureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int LCID = (int)value;
            return new CultureInfo(LCID).NativeName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
