using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChameleonCoder.UI
{
    /// <summary>
    /// a WPF numeric updown control to be used by CC and plugins
    /// </summary>
    public sealed partial class NumericUpDown : UserControl
    {
        /// <summary>
        /// creates a new instance of the control
        /// </summary>
        public NumericUpDown()
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

        /// <summary>
        /// gets or sets the number shown in the control
        /// </summary>
        [System.ComponentModel.Bindable(true, System.ComponentModel.BindingDirection.TwoWay)]
        public int Number
        {
            get { return (int)GetValue(NumberProperty); }
            set { SetValue(NumberProperty, value); }
        }

        /// <summary>
        /// the DependencyProperty for the <see cref="Number"/> CLR property
        /// </summary>
        public static readonly DependencyProperty NumberProperty
            = DependencyProperty.Register(  "Number",
                                            typeof(int),
                                            typeof(NumericUpDown),
                                            new FrameworkPropertyMetadata() { BindsTwoWayByDefault = true });

        /// <summary>
        /// gets or sets the maximum number to be entered by the user
        /// </summary>
        /// <remarks>The user may paste some higher number,
        /// however, <see cref="Number"/> will only accept numbers less or equal to this.</remarks>
        public int MaxNumber
        {
            get { return (int)GetValue(MaxNumberProperty); }
            set { SetValue(MaxNumberProperty, value); }
        }

        /// <summary>
        /// the DependencyProperty for the <see cref="MaxNumber"/> CLR property
        /// </summary>
        public static readonly DependencyProperty MaxNumberProperty
            = DependencyProperty.Register(  "MaxNumber",
                                            typeof(int),
                                            typeof(NumericUpDown),
                                            new PropertyMetadata(Int32.MaxValue));

        /// <summary>
        /// gets or sets the minimum number to be entered by the user
        /// </summary>
        /// <remarks>The user may paste some smaller number,
        /// however, <see cref="Number"/> will only accept numbers greater or equal to this.</remarks>
        public int MinNumber
        {
            get { return (int)GetValue(MinNumberProperty); }
            set { SetValue(MinNumberProperty, value); }
        }

        /// <summary>
        /// the DependencyProperty for the <see cref="MinNumber"/> CLR property
        /// </summary>
        public static readonly DependencyProperty MinNumberProperty
            = DependencyProperty.Register(  "MinNumber",
                                            typeof(int),
                                            typeof(NumericUpDown),
                                            new PropertyMetadata(Int32.MinValue));
    }
}
