using System;
using System.Windows;

namespace GuidCreator
{
    public partial class CreatorView : Window
    {
        public CreatorView()
        {
            InitializeComponent();
            this.Invoke(null, null);
            this.ShowDialog();
        }

        public void Invoke(object sender, EventArgs e)
        {
            Guid g = Guid.NewGuid();
            this.GuidB.Text = g.ToString("b");
            this.GuidD.Text = g.ToString("d");
            this.GuidN.Text = g.ToString("n");
            this.GuidP.Text = g.ToString("p");
            this.GuidX.Text = g.ToString("x");
        }

        public void Enter(object sender, EventArgs e)
        {
            InputBox box = new InputBox();
            if (box.ShowDialog() == true)
            {
                Guid g = box.Guid;
                this.GuidB.Text = g.ToString("b");
                this.GuidD.Text = g.ToString("d");
                this.GuidN.Text = g.ToString("n");
                this.GuidP.Text = g.ToString("p");
                this.GuidX.Text = g.ToString("x");
            }
        }
    }
}
