using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(IResource), typeof(ImageSource))]
    class ResourceImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;
            IResolvable link;

            if (resource != null)
            {
                while ((link = (resource as IResolvable)) != null && link.shouldResolve)
                    resource = link.Resolve();

                return resource.Icon;
            }

            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
