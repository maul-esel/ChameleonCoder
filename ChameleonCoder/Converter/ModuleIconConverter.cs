using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(Resources.Base.ResourceBase), typeof(ImageSource))]
    class ModuleIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Resources.Base.ResourceBase resource = value as Resources.Base.ResourceBase;

            while (resource is LinkResource)
                resource = (resource as LinkResource).Resolve();

            if (resource is CodeResource)
            {
                try { return App.Host.GetModule((resource as CodeResource).Language).Icon; }
                catch (NullReferenceException) { }
            }

            else if (resource is ProjectResource)
            {
                try { return App.Host.GetModule((resource as ProjectResource).Language).Icon; }
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
