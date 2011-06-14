using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(ResourceType), typeof(string))]
    class ResourceTypeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResourceType type = (ResourceType)(int)value;

            switch (type)
            {
                case ResourceType.file: return @"Images\ResourceType\file.png";
                case ResourceType.code: return @"Images\ResourceType\code.png";
                case ResourceType.library: return @"Images\ResourceType\library.png";
                case ResourceType.project: return @"Images\ResourceType\project.png";
                case ResourceType.task: return @"Images\ResourceType\task.png";
                case ResourceType.link: break; // TODO: how to get the corresponding LinkResource instance?
                                               // --> Resolve().Type
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
