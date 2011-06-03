﻿namespace AHKScriptsMan
{
    /// <summary>
    /// represents the main window
    /// </summary>
    partial class MainWin
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.Container1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip0 = new System.Windows.Forms.ToolStrip();
            this.toolbutton0_1 = new System.Windows.Forms.ToolStripButton();
            this.toolbutton0_2 = new System.Windows.Forms.ToolStripButton();
            this.toolbutton0_3 = new System.Windows.Forms.ToolStripButton();
            this.toolbutton0_4 = new System.Windows.Forms.ToolStripButton();
            this.TreeView = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolbutton1_1 = new System.Windows.Forms.ToolStripButton();
            this.toolbutton1_2 = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.scintilla1 = new ScintillaNet.Scintilla();
            ((System.ComponentModel.ISupportInitialize)(this.Container1)).BeginInit();
            this.Container1.Panel1.SuspendLayout();
            this.Container1.Panel2.SuspendLayout();
            this.Container1.SuspendLayout();
            this.toolStrip0.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).BeginInit();
            this.SuspendLayout();
            // 
            // Container1
            // 
            this.Container1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Container1.Location = new System.Drawing.Point(0, 0);
            this.Container1.Name = "Container1";
            // 
            // Container1.Panel1
            // 
            this.Container1.Panel1.Controls.Add(this.toolStrip0);
            this.Container1.Panel1.Controls.Add(this.TreeView);
            // 
            // Container1.Panel2
            // 
            this.Container1.Panel2.Controls.Add(this.panel1);
            this.Container1.Panel2.Controls.Add(this.panel2);
            this.Container1.Size = new System.Drawing.Size(1173, 640);
            this.Container1.SplitterDistance = 391;
            this.Container1.TabIndex = 0;
            // 
            // toolStrip0
            // 
            this.toolStrip0.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbutton0_1,
            this.toolbutton0_2,
            this.toolbutton0_3,
            this.toolbutton0_4});
            this.toolStrip0.Location = new System.Drawing.Point(0, 0);
            this.toolStrip0.Name = "toolStrip0";
            this.toolStrip0.Size = new System.Drawing.Size(391, 25);
            this.toolStrip0.TabIndex = 1;
            this.toolStrip0.Text = "toolStrip2";
            // 
            // toolbutton0_1
            // 
            this.toolbutton0_1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbutton0_1.Image = ((System.Drawing.Image)(resources.GetObject("toolbutton0_1.Image")));
            this.toolbutton0_1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbutton0_1.Name = "toolbutton0_1";
            this.toolbutton0_1.Size = new System.Drawing.Size(23, 22);
            // 
            // toolbutton0_2
            // 
            this.toolbutton0_2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbutton0_2.Image = ((System.Drawing.Image)(resources.GetObject("toolbutton0_2.Image")));
            this.toolbutton0_2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbutton0_2.Name = "toolbutton0_2";
            this.toolbutton0_2.Size = new System.Drawing.Size(23, 22);
            this.toolbutton0_2.Text = "toolStripButton1";
            // 
            // toolbutton0_3
            // 
            this.toolbutton0_3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbutton0_3.Image = ((System.Drawing.Image)(resources.GetObject("toolbutton0_3.Image")));
            this.toolbutton0_3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbutton0_3.Name = "toolbutton0_3";
            this.toolbutton0_3.Size = new System.Drawing.Size(23, 22);
            this.toolbutton0_3.Text = "toolStripButton2";
            // 
            // toolbutton0_4
            // 
            this.toolbutton0_4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbutton0_4.Image = ((System.Drawing.Image)(resources.GetObject("toolbutton0_4.Image")));
            this.toolbutton0_4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbutton0_4.Name = "toolbutton0_4";
            this.toolbutton0_4.Size = new System.Drawing.Size(23, 22);
            this.toolbutton0_4.Text = "toolStripButton3";
            // 
            // TreeView
            // 
            this.TreeView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TreeView.Location = new System.Drawing.Point(0, 28);
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(391, 612);
            this.TreeView.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.toolStrip1);
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(778, 640);
            this.panel1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolbutton1_1,
            this.toolbutton1_2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(778, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolbutton1_1
            // 
            this.toolbutton1_1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbutton1_1.Image = ((System.Drawing.Image)(resources.GetObject("toolbutton1_1.Image")));
            this.toolbutton1_1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbutton1_1.Name = "toolbutton1_1";
            this.toolbutton1_1.Size = new System.Drawing.Size(23, 22);
            // 
            // toolbutton1_2
            // 
            this.toolbutton1_2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolbutton1_2.Image = ((System.Drawing.Image)(resources.GetObject("toolbutton1_2.Image")));
            this.toolbutton1_2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolbutton1_2.Name = "toolbutton1_2";
            this.toolbutton1_2.Size = new System.Drawing.Size(23, 22);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView1.Location = new System.Drawing.Point(0, 28);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(778, 612);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.scintilla1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(778, 640);
            this.panel2.TabIndex = 1;
            // 
            // scintilla1
            // 
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scintilla1.Location = new System.Drawing.Point(0, 36);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(778, 604);
            this.scintilla1.Styles.BraceBad.FontName = "Verdana";
            this.scintilla1.Styles.BraceLight.FontName = "Verdana";
            this.scintilla1.Styles.ControlChar.FontName = "Verdana";
            this.scintilla1.Styles.Default.FontName = "Verdana";
            this.scintilla1.Styles.IndentGuide.FontName = "Verdana";
            this.scintilla1.Styles.LastPredefined.FontName = "Verdana";
            this.scintilla1.Styles.LineNumber.FontName = "Verdana";
            this.scintilla1.Styles.Max.FontName = "Verdana";
            this.scintilla1.TabIndex = 0;
            // 
            // MainWin
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1173, 640);
            this.Controls.Add(this.Container1);
            this.Font = new System.Drawing.Font("Cambria", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainWin";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "AHK.ScriptsMan alpha 1";
            this.Container1.Panel1.ResumeLayout(false);
            this.Container1.Panel1.PerformLayout();
            this.Container1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Container1)).EndInit();
            this.Container1.ResumeLayout(false);
            this.toolStrip0.ResumeLayout(false);
            this.toolStrip0.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scintilla1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.SplitContainer Container1;
        public System.Windows.Forms.TreeView TreeView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip0;
        private System.Windows.Forms.Panel panel2;
        internal ScintillaNet.Scintilla scintilla1;
        internal System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ToolStripButton toolbutton1_2;
        internal System.Windows.Forms.ToolStrip toolStrip1;
        internal System.Windows.Forms.ToolStripButton toolbutton1_1;
        private System.Windows.Forms.ToolStripButton toolbutton0_2;
        private System.Windows.Forms.ToolStripButton toolbutton0_3;
        private System.Windows.Forms.ToolStripButton toolbutton0_4;
        internal System.Windows.Forms.ToolStripButton toolbutton0_1;



    }
}

