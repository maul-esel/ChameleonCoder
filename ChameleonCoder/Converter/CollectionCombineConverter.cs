using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace ChameleonCoder.Converter
{
    internal class CollectionCombineConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type target, object param, System.Globalization.CultureInfo culture)
        {
            var collection = new System.Collections.ObjectModel.ObservableCollection<IComponent>();

            foreach (var item in values)
            {
                var col = item as IEnumerable<IComponent>;
                if (col != null)
                    foreach (var comp in col)
                        collection.Add(comp);
            }

            return collection;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
