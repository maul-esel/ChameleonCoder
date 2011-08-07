using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources.Interfaces;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(IResource), typeof(ImageSource))]
    internal sealed class ModuleIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;
            IResolvable link;

            while ((link = resource as IResolvable) != null && link.shouldResolve)
                resource = link.Resolve();

            if (resource is ILanguageResource)
            {
                ILanguageModule module;
                if (PluginManager.TryGetModule((resource as ILanguageResource).language, out module) && module.Icon != null)
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
