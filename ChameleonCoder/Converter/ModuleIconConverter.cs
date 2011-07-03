using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Resources;
using ChameleonCoder.LanguageModules;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(Resources.Base.ResourceBase), typeof(ImageSource))]
    class ModuleIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Resources.Base.ResourceBase resource = value as Resources.Base.ResourceBase;

            while (resource is IResolvable)
                resource = (resource as IResolvable).Resolve();

            if (resource is CodeResource)
            {
                try { return LanguageModuleHost.GetModule((resource as CodeResource).language).Icon; }
                catch (NullReferenceException) { }
            }

            else if (resource is ProjectResource)
            {
                try { return LanguageModuleHost.GetModule((resource as ProjectResource).language).Icon; }
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
