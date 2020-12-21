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
namespace NotepadDIY.Components
{
    public partial class DocMapTextBox : UserControl
    {
        //property
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
            this.documentMap1.Size = SizeExt.Mult(this.fastColoredTextBox1.Size, DocumentMapSize);
            //Console.WriteLine(this.documentMap1.Size);
            this.documentMap1.Location = new Point(this.fastColoredTextBox1.Size.Width - this.documentMap1.Size.Width - System.Windows.Forms.SystemInformation.VerticalScrollBarWidth, 0);

            //Console.WriteLine("move");

        }
    }
}
