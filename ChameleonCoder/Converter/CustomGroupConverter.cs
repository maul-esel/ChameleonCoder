using System;
using System.Windows.Data;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(IResource), typeof(Type))]
    class CustomGroupConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;
            IResolvable link;

            if (resource != null)
            {
                while ((link = (resource as IResolvable)) != null && link.shouldResolve)
                    resource = link.Resolve();

                try { return resource.GetType(); }
                catch (NullReferenceException) { }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
