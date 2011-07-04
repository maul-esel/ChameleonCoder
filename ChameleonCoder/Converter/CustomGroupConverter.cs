using System;
using System.Windows.Data;
using ChameleonCoder.Resources.Interfaces;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(IResource), typeof(Type))]
    class CustomGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;

            if (resource != null)
            {
                while (resource is IResolvable)
                    resource = (resource as IResolvable).Resolve();

                return resource.GetType();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
