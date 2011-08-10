using System;
using System.Windows.Data;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(Plugins.IPlugin), typeof(bool))]
    internal class PluginDisabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Properties.Settings.Default.DisabledPlugins.Contains((value as Plugins.IPlugin).Identifier.ToString("n"));
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
