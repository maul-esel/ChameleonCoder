using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(IResource), typeof(ImageSource))]
    internal sealed class ResourceImageConverter : IValueConverter
    {
        internal object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;
            IResource original = resource;
            IResolvable link;

            if (resource != null)
            {
                while ((link = (resource as IResolvable)) != null && link.shouldResolve)
                    resource = link.Resolve();

                if (resource != null)
                    return resource.Icon.GetAsFrozen();
                else
                    return original.Icon.GetAsFrozen();
            }
            return null;
        }

        internal object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
