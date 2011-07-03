using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Resources;
using ChameleonCoder.LanguageModules;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(IResource), typeof(ImageSource))]
    class ModuleIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;

            while (resource is IResolvable)
                resource = (resource as IResolvable).Resolve();

            if (resource is ILanguageResource)
            {
                try { return LanguageModuleHost.GetModule((resource as ILanguageResource).language).Icon; }
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
