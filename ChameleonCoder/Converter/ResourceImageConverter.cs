using System;
using System.Windows.Data;
using ChameleonCoder.Resources.Base;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(IResource), typeof(string))]
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

                /*switch (resource.GetType())
                {
                    case ResourceType.file: return @"Images\ResourceType\file.png";
                    case ResourceType.code: return @"Images\ResourceType\code.png";
                    case ResourceType.library: return @"Images\ResourceType\library.png";
                    case ResourceType.project: return @"Images\ResourceType\project.png";
                    case ResourceType.task: return @"Images\ResourceType\task.png";
                }*/
            }

            
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
