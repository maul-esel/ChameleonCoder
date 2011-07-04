using System;
using System.Windows.Data;
using ChameleonCoder.Resources.Interfaces;
using System.Windows.Media;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(Type), typeof(ImageSource))]
    sealed class ResourceTypeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Type type = value as Type;
            IResolvable link;
            IResource resource;

            if (type != null)
            {
                while ((link = (resource = Activator.CreateInstance(type) as IResource) as IResolvable) != null && link.shouldResolve)
                    type = link.Resolve().GetType();

                return (Activator.CreateInstance(type) as IResource).TypeIcon;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
