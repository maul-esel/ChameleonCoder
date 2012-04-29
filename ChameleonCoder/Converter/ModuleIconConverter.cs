using System;
using System.Windows.Data;
using System.Windows.Media;
using ChameleonCoder.Plugins;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Converter
{
    /// <summary>
    /// converts an IResource instance to the corresponding Language module's icon.
    /// </summary>
    [ValueConversion(typeof(IResource), typeof(ImageSource)), System.Runtime.InteropServices.ComVisible(false)]
    internal sealed class ModuleIconConverter : IValueConverter
    {
        /// <summary>
        /// converts the instance to the icon.
        /// </summary>
        /// <param name="value">the IResource instance</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture"></param>
        /// <returns>the module's icon</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IResource resource = value as IResource;

            // check if it is really an IResource instance
            if (resource != null)
            {
                // if so, cast the resource to an ILanguageResource instance
                ILanguageResource langRes = resource as ILanguageResource;
                // if it is an ILanguageResource
                if (langRes != null)
                {
                    ILanguageModule module;
                    // check if the corresponding Language module is registered and it has a non-null icon
                    if (ChameleonCoderApp.RunningObject.PluginMan.TryGetModule(langRes.Language, out module) && module.Icon != null)
                        // if so: return its icon
                        return module.Icon.GetAsFrozen(); // use GetAsFrozen() to avoid multi-threading issues
                }
                // if it is not an ILanguageResource instance or the module is not registered or it's icon is null:
                return null; // return null
            }
            // if value is not an IResource instance or it was already null, throw an exception
            throw new ArgumentException("'value' is either null or not an IResource instance", "value");
        }

        /// <summary>
        /// this converter does not support converting back.
        /// </summary>
        /// <param name="value">not used</param>
        /// <param name="targetType">not used</param>
        /// <param name="parameter">not used</param>
        /// <param name="culture">not used</param>
        /// <returns>null</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
