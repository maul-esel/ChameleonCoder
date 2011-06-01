using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AHKScriptsMan
{
    public delegate void DialogEvent();

    public partial class MsgBoxCreator : Form
    {
        int options;
        string title;
        string text;
        int timeout;
        
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
            this.button4.Click += new EventHandler(button4_Click);

            OnInsertCode += insert;
            OnSaveCode += save;

        }

        void button4_Click(object sender, EventArgs e)
        {
            if (this.OnSaveCode != null)
            {
                this.OnSaveCode();
            }
        }

        void button3_Click(object sender, EventArgs e)
        {
            this.button1_Click(sender, e);
            this.MsgBox(this.options, this.title, this.text, this.timeout);
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
            this.options = flags;
            this.title = this.title_textbox.Text;
            this.text = this.text_textbox.Text;
            this.timeout = (int)this.numericUpDown1.Value;

            StringBuilder builder = new StringBuilder(this.title_textbox.Text);
            builder.Replace("\n", "`n");
            builder.Replace(",", "`,");
            builder.Replace(";", "`;");
            string title = builder.ToString();
            
            this.output_textbox.Text = "MsgBox " + flags + ", " + title + ", " + this.text_textbox.Text + ", " + this.numericUpDown1.Value.ToString();
        }

        /// <summary>
        /// parses code and creates the MsgBox.
        /// This code is taken from the IronAHK project: www.ironahk.net
        /// </summary>
        /// <param name="Options"></param>
        /// <param name="Title"></param>
        /// <param name="Text"></param>
        /// <param name="Timeout"></param>
        private void MsgBox(int Options, string Title, string Text, int Timeout)
        {
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            switch (Options & 0xf)
            {
                case 0: buttons = MessageBoxButtons.OK; break;
                case 1: buttons = MessageBoxButtons.OKCancel; break;
                case 2: buttons = MessageBoxButtons.AbortRetryIgnore; break;
                case 3: buttons = MessageBoxButtons.YesNoCancel; break;
                case 4: buttons = MessageBoxButtons.YesNo; break;
                case 5: buttons = MessageBoxButtons.RetryCancel; break;
                //case 6: /* Cancel/Try Again/Continue */ ; break;
                //case 7: /* Adds a Help button */ ; break; // help done differently
            }

            MessageBoxIcon icon = MessageBoxIcon.None;
            switch (Options & 0xf0)
            {
                case 16: icon = MessageBoxIcon.Hand; break;
                case 32: icon = MessageBoxIcon.Question; break;
                case 48: icon = MessageBoxIcon.Exclamation; break;
                case 64: icon = MessageBoxIcon.Asterisk; break;
            }

            MessageBoxDefaultButton defaultbutton = MessageBoxDefaultButton.Button1;
            switch (Options & 0xf00)
            {
                case 256: defaultbutton = MessageBoxDefaultButton.Button2; break;
                case 512: defaultbutton = MessageBoxDefaultButton.Button3; break;
            }

            var options = default(MessageBoxOptions);
            switch (Options & 0xf0000)
            {
                case 131072: options = MessageBoxOptions.DefaultDesktopOnly; break;
                case 262144: options = MessageBoxOptions.ServiceNotification; break;
                case 524288: options = MessageBoxOptions.RightAlign; break;
                case 1048576: options = MessageBoxOptions.RtlReading; break;
            }

            bool help = (Options & 0xf000) == 16384;

            if (string.IsNullOrEmpty(Title))
            {
                var script = Environment.GetEnvironmentVariable("SCRIPT");

                if (!string.IsNullOrEmpty(script) && File.Exists(script))
                    Title = Path.GetFileName(script);
            }

            MessageBox.Show(Text, Title, buttons, icon, defaultbutton, options, help);

        }
    }
}
