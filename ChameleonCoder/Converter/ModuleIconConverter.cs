using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.LanguageModules;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(IResource), typeof(ImageSource))]
    internal sealed class ModuleIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;

            while (resource is IResolvable)
                resource = (resource as IResolvable).Resolve();

            if (resource is ILanguageResource)
            {
                ILanguageModule module = LanguageModuleHost.GetModule((resource as ILanguageResource).language);
                if (module != null)
                    return module.Icon.GetAsFrozen();
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
