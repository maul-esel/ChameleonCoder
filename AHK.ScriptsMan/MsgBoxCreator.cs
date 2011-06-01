using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AHKScriptsMan
{
    public delegate void DialogEvent();

    public partial class MsgBoxCreator : Form
    {
        /// <summary>
        /// a delegate that is called when the user clicks "Insert Code"
        /// </summary>
        /// <returns>(void)</returns>
        public event DialogEvent OnInsertCode;

        /// <summary>
        /// a delegate that is called when the user clicks "Save code"
        /// </summary>
        /// <returns>(void)</returns>
        public event DialogEvent OnSaveCode;


        public MsgBoxCreator(DialogEvent insert, DialogEvent save)
        {
            InitializeComponent();

            this.radioButton1.Image = Bitmap.FromHicon(SystemIcons.Hand.Handle);
            this.radioButton2.Image = Bitmap.FromHicon(SystemIcons.Question.Handle);
            this.radioButton3.Image = Bitmap.FromHicon(SystemIcons.Exclamation.Handle);
            this.radioButton4.Image = Bitmap.FromHicon(SystemIcons.Information.Handle);
            this.numericUpDown1.Value = -1;

            this.button1.Click += new EventHandler(button1_Click);
            this.button2.Click += new EventHandler(button2_Click);
            this.button3.Click += new EventHandler(button3_Click);

            OnInsertCode += insert;
            OnSaveCode += save;

        }

        void button3_Click(object sender, EventArgs e)
        {
            if (this.OnSaveCode != null)
            {
                this.OnSaveCode();
            }
        }

        void button2_Click(object sender, EventArgs e)
        {
            if (this.OnInsertCode != null)
            {
                this.OnInsertCode();
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            int flags = 0;
            switch (this.modal_comboBox.SelectedIndex)
            {
                case 1: flags += 8192;
                    break;
                case 2: flags += 4096;
                    break;
                case 3: flags += 262144;
                    break;
                case 4: flags += 131072;
                    break;
                default:
                    flags += 0;
                    break;
            }

            if (this.radioButton0.Checked)
            {
                flags += 0;
            }
            else if (this.radioButton1.Checked)
            {
                flags += 16;
            }
            else if (this.radioButton2.Checked)
            {
                flags += 32;
            }
            else if (this.radioButton3.Checked)
            {
                flags += 48;
            }
            else if (this.radioButton4.Checked)
            {
                flags += 64;
            }

            switch (this.button_combobox.SelectedIndex)
            {
                case 1: flags += 1;
                    break;
                case 2: flags += 2;
                    break;
                case 3: flags += 3;
                    break;
                case 4: flags += 4;
                    break;
                case 5: flags += 5;
                    break;
                case 6: flags += 6;
                    break;
                default: flags += 0;
                    break;
            }

            switch (this.defbutton_combobox.SelectedIndex)
            {
                case 1: flags += 256;
                    break;
                case 2: flags += 512;
                    break;
                default: flags += 0;
                    break;
            }

            if (this.right_checkbox.Checked)
            {
                flags += 524288;
            }

            if (this.rtl_checkbox.Checked)
            {
                flags += 1048576;
            }

            StringBuilder builder = new StringBuilder(this.title_textbox.Text);
            builder.Replace("\n", "`n");
            builder.Replace(",", "`,");
            builder.Replace(";", "`;");
            string title = builder.ToString();

            
            this.output_textbox.Text = "MsgBox " + flags + ", " + title + ", " + this.text_textbox.Text;
        }

        //public void EmptyMethod()
        //{
        //}
    }
}
