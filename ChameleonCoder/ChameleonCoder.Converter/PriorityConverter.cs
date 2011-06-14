using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ChameleonCoder.Resources;

namespace ChameleonCoder.Converter
{
    [ValueConversion(typeof(ProjectResource.ProjectPriority), typeof(string))]
    public sealed class PriorityConverter : IValueConverter // convert priority to image
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ProjectResource.ProjectPriority priority = ((ProjectResource)value).Priority;

            switch (priority)
            {
                case ProjectResource.ProjectPriority.basic: return @"Images\Priority\low.png";
                case ProjectResource.ProjectPriority.middle: return @"Images\Priority\middle.png";
                case ProjectResource.ProjectPriority.high: return @"Images\Priority\high.png";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
