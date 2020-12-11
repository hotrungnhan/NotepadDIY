using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotepadDIY.Components
{
    public partial class DocMapTextBox : UserControl
    {
        public DocMapTextBox()
        {
            InitializeComponent();
        }

        private void DocMapTextBox_Resize(object sender, EventArgs e)
        {
            //Console.WriteLine("move");
        }
    }
}
