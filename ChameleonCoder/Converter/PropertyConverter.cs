using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Text;
using ChameleonCoder.Resources;
using ChameleonCoder.Resources.Base;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(ResourceBase), typeof(List<ResourceProperty>))]
    class PropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<ResourceProperty> list = new List<ResourceProperty>();
            ResourceBase resource = value as ResourceBase;

            if (resource != null)
            {
                list.Add(new ResourceProperty() { Value = resource.DataFile, Name = "datafile", Group = "general" });
                list.Add(new ResourceProperty() { Value = resource.GUID.ToString(), Name = "GUID", Group = "general" });
            }

            FileResource file = resource as FileResource;
            if (file != null)
            {
                list.Add(new ResourceProperty() { Value = file.Path, Name = "path", Group = "file" });
            }

            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    class ResourceProperty
    {
        public string Value;
        public string Name;
        public string Group;
    }
}
