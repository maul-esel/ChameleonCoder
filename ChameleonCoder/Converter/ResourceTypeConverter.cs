using System;
using System.Windows.Data;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(ResourceType), typeof(string))]
    public sealed class ResourceTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ResourceType type = (ResourceType)(int)value;

            switch (type)
            {
                case ResourceType.resource: return "resource";
                case ResourceType.link: return "link";
                case ResourceType.file: return "file";
                case ResourceType.code: return "code";
                case ResourceType.library: return "library";
                case ResourceType.project: return "project"; // todo: return language names
                case ResourceType.task: return "task";
            }           

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
