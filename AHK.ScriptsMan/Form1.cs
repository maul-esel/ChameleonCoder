using System;
using System.Windows.Forms;

namespace AHKScriptsMan
{
    public partial class MainWin : Form
    {
        public ListViewGroup[] groups { get; set; }

        public MainWin()
        {
            InitializeComponent();
            
            this.listView1.Columns.Add("Name");
            this.listView1.Columns.Add("description");
            this.listView1.View = View.Details;

            this.listView1.ShowGroups = true;
            this.groups = new ListViewGroup[5];
            groups[0] = this.listView1.Groups.Add("files", string.Empty);
            groups[1] = this.listView1.Groups.Add("codes", string.Empty);
            groups[2] = this.listView1.Groups.Add("libraries", string.Empty);
            groups[3] = this.listView1.Groups.Add("projects", string.Empty);
            groups[4] = this.listView1.Groups.Add("tasks", string.Empty);

            this.TreeView.ImageList = Window.DataProvider.GetImageList(9, 5);
            this.TreeView.PathSeparator = "\\";

            this.DragDrop += new DragEventHandler(Program.Window_DragDrop); // geeignet für Dateien?
            this.TreeView.Click += new EventHandler(Program.TreeView_Click);

            this.UpdateLanguage();

            //this.scintilla1.StyleNeeded += ...
            this.scintilla1.Lexing.Lexer = ScintillaNet.Lexer.Container;

            
        }

        public void UpdateLanguage()
        {
            this.toolbutton0_1.Text = Localization.get_string("Item_Options");
        }

    }
}
