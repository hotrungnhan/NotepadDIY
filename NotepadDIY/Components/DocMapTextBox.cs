using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NotepadDIY.Components.Ext;
using FastColoredTextBoxNS;
using FarsiLibrary.Win;
using System.IO;
namespace NotepadDIY.Components
{
    public partial class DocMapTextBox : UserControl
    {
        //property
        bool is_change = false;
        static float BindModeSizeMB = 1;
        public double DocumentMapSize { get; set; } = 0.2;
        public bool RulerEnable
        {
            get { return this.ruler1.Visible; }
            set { this.ruler1.Visible = value; }
        }
        public FastColoredTextBox TextBox
        {

            get { return this.fastColoredTextBox1; }
        }
        //
        //Method
        public DocMapTextBox()
        {
            InitializeComponent();
        }
        private void DocMapTextBox_Resize(object sender, EventArgs e)
        {
            if (RulerEnable)
            {
                this.documentMap1.Size = SizeExt.Mult(this.fastColoredTextBox1.Size, DocumentMapSize);
                this.documentMap1.Location = new Point(this.fastColoredTextBox1.Size.Width - this.documentMap1.Size.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, ruler1.Height);
            }
            else
            {
                this.documentMap1.Size = SizeExt.Mult(this.fastColoredTextBox1.Size, DocumentMapSize);
                this.documentMap1.Location = new Point(this.fastColoredTextBox1.Size.Width - this.documentMap1.Size.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, 0);
            }
        }
        public void LoadFile(string path)
        {
            if (File.Exists(path))
            {

                var filelength = new FileInfo(path).Length;
                if (filelength < BindModeSizeMB * 1024 * 1024)
                {
                    this.fastColoredTextBox1.OpenFile(path);
                }
                else
                {
                    if (MessageBox.Show("File is Too big .Do you want to open file as Bindding mode ? ", "DOCmap", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        this.fastColoredTextBox1.OpenBindingFile(path, Encoding.UTF8);
                }
            }
            else
            {
                MessageBox.Show("file not exists");
            }
            is_change = false;
        }


        private void ruler1_VisibleChanged(object sender, EventArgs e)
        {
            if (RulerEnable)
            {
                this.documentMap1.Size = SizeExt.Mult(this.fastColoredTextBox1.Size, DocumentMapSize);
                this.documentMap1.Location = new Point(this.fastColoredTextBox1.Size.Width - this.documentMap1.Size.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, ruler1.Height);
            }
            else
            {
                this.documentMap1.Size = SizeExt.Mult(this.fastColoredTextBox1.Size, DocumentMapSize);
                this.documentMap1.Location = new Point(this.fastColoredTextBox1.Size.Width - this.documentMap1.Size.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, 0);
            }
        }

        private void DocMapTextBox_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control is FastColoredTextBox)
            {
                Console.WriteLine("Run");
                this.fastColoredTextBox1.CloseBindingFile();
                this.fastColoredTextBox1.Dispose();
            }
        }

        private void fastColoredTextBox1_TextChanging(object sender, TextChangingEventArgs e)
        {
            var par = this.Parent as FATabStripItem;

            if (par != null && is_change == false)
            {
                par.Title = par.Title + "*";
                is_change = true;
            }
        }
        public string getFastText()
        {
            return fastColoredTextBox1.Text;
        }
    }

}
