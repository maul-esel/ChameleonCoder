using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ChameleonCoder
{
    internal sealed partial class CreateResource : Form
    {
        internal CreateResource()
        {
            InitializeComponent();

            Program.Gui.Enabled = false;

            this.Owner = Program.Gui;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Text = Localization.get_string("Action_New");
            this.FormClosed += new FormClosedEventHandler(OnClosed);

            this.button1.Text = Localization.get_string("Action_NewFile");
            this.button2.Text = Localization.get_string("Action_NewCode");
            this.button3.Text = Localization.get_string("Action_NewLibrary");
            this.button4.Text = Localization.get_string("Action_NewProject");
            this.button5.Text = Localization.get_string("Action_NewTask");

            this.button1.Click += new EventHandler(cFile.Create);
            this.button2.Click += new EventHandler(cCodeFile.Create);
            this.button3.Click += new EventHandler(cLibrary.Create);
            this.button4.Click += new EventHandler(cProject.Create);
            this.button5.Click += new EventHandler(cTask.Create);

            Program.Selector = this;
            this.Show();
        }

        internal void OnClosed(object sender, EventArgs e)
        {
            Program.Gui.Enabled = true;
            this.Dispose();
        }
    }
}
