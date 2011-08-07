using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Text;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Converter
{
    public class ResourcePropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<Resources.PropertyDescription> list = new List<Resources.PropertyDescription>();
            Type type = value.GetType();

            foreach (var property in type.GetProperties())
            {
                ResourcePropertyAttribute attr = (ResourcePropertyAttribute)Attribute.GetCustomAttribute(property, typeof(ResourcePropertyAttribute));
                var getMethod = property.GetGetMethod();

                if (attr == null || getMethod == null)
                    continue;

                var propertyValue = getMethod.Invoke(value, null);

                // getting the group name
                string group = string.Empty;
                if (attr.Group == ResourcePropertyGroup.General)
                    group = Properties.Resources.PropertyGroup_General;
                else if (attr.Group == ResourcePropertyGroup.CurrentClass)
                    group = Resources.Management.ResourceTypeManager.GetInfo(type).DisplayName;
                else if (attr.Group == ResourcePropertyGroup.ThisClass)
                    group = Resources.Management.ResourceTypeManager.GetInfo(property.DeclaringType).DisplayName;

                // getting the property name
                string name = attr.Name;
                if (attr.IsReferenceName)
                {
                    var nameProperty = type.GetProperty(attr.Name, typeof(string));
                    if (nameProperty != null && nameProperty.GetGetMethod() != null)
                    {
                        string _name = (string)nameProperty.GetGetMethod().Invoke(value, null);
                        if (!string.IsNullOrWhiteSpace(_name))
                            name = _name;
                    }
                }

                list.Add(new Resources.PropertyDescription(name, group, property, value, attr.IsReadOnly));
            }

            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
