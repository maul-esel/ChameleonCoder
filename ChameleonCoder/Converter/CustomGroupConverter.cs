using System;
using System.Windows.Data;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(ResourceBase), typeof(ResourceType))]
    class CustomGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResourceBase resource = value as ResourceBase;

            if (resource != null)
            {
                while (resource is IResolvable)
                    resource = (resource as IResolvable).Resolve();

                return resource.Type;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
