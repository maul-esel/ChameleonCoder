using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Text;
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
                while (resource is LinkResource)
                    resource = (resource as LinkResource).Resolve();

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
