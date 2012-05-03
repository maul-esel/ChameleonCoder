using System.Windows;

namespace ChameleonCoder.UI.Converters
{
    public abstract class AppConverterBase : DependencyObject
    {
        public static readonly DependencyProperty AppProperty = DependencyProperty.Register("App", typeof(IChameleonCoderApp), typeof(AppConverterBase));

        public IChameleonCoderApp App
        {
            get
            {
                return (IChameleonCoderApp)GetValue(AppProperty);
            }
            set
            {
                SetValue(AppProperty, value);
            }
        }
    }
}
