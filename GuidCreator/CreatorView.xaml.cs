using System;
using System.Windows;

namespace GuidCreator
{
    public partial class CreatorView : Window
    {
        public Guid currentGuid { get; private set; }

        public CreatorView()
        {
            InitializeComponent();
            DataContext = currentGuid = Guid.NewGuid();
            this.ShowDialog();
        }

        public void Invoke(object sender, EventArgs e)
        {
            DataContext = currentGuid = Guid.NewGuid();
        }

        public void Enter(object sender, EventArgs e)
        {
            InputBox box = new InputBox();
            if (box.ShowDialog() == true)
                DataContext = currentGuid = box.Guid;
        }
    }
}
