using System;
using System.Windows.Data;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(ResourceType), typeof(string))]
    sealed class ResourceTypeImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResourceType type = (ResourceType)(int)value;

            switch (type)
            {
                case ResourceType.link: return null;
                case ResourceType.file: return @"Images\ResourceType\file.png";
                case ResourceType.code: return @"Images\ResourceType\code.png";
                case ResourceType.library: return @"Images\ResourceType\library.png";
                case ResourceType.project: return @"Images\ResourceType\project.png";
                case ResourceType.task: return @"Images\ResourceType\task.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
