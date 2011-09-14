using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace ChameleonCoder.Interaction
{
    /// <summary>
    /// Interaktionslogik für CCNumericUpDown.xaml
    /// </summary>
    public sealed partial class CCNumericUpDown : UserControl
    {
        public CCNumericUpDown()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void InputEventHandler(object sender, TextCompositionEventArgs e)
        {
            int newNumber;
            if (!Int32.TryParse(e.Text, out newNumber))
                e.Handled = true;
        }

        private void TextBoxChanged(object sender, EventArgs e)
        {
            int newNumber;
            if (Int32.TryParse(box.Text, out newNumber))
                if (newNumber >= MinNumber && newNumber <= MaxNumber)
                    Number = newNumber;
        }

        private void Increase(object sender, EventArgs e)
        {
            if (Number < MaxNumber)
                Number++;
            else
                Number = MinNumber;
        }

        private void Decrease(object sender, EventArgs e)
        {
            if (Number > MinNumber)
                Number--;
            else
                Number = MaxNumber;
        }

        [System.ComponentModel.Bindable(true, System.ComponentModel.BindingDirection.TwoWay)]
        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        public static readonly DependencyProperty NumberProperty
            = DependencyProperty.Register(  "Number",
                                            typeof(int),
                                            typeof(CCNumericUpDown),
                                            new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true });

        public int MaxNumber
        {
            get { return (int)GetValue(MaxNumberProperty); }
            set { SetValue(MaxNumberProperty, value); }
        }

        public static readonly DependencyProperty MaxNumberProperty
            = DependencyProperty.Register(  "MaxNumber",
                                            typeof(int),
                                            typeof(CCNumericUpDown),
                                            new PropertyMetadata(Int32.MaxValue));

        public int MinNumber
        {
            get { return (int)GetValue(MinNumberProperty); }
            set { SetValue(MinNumberProperty, value); }
        }

        public static readonly DependencyProperty MinNumberProperty
            = DependencyProperty.Register(  "MinNumber",
                                            typeof(int),
                                            typeof(CCNumericUpDown),
                                            new PropertyMetadata(Int32.MinValue));
    }
}
