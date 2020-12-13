using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NotepadDIY.Components;
using NotepadDIY.Properties;
using FarsiLibrary.Win;
namespace NotepadDIY
{

    public partial class Form1 : Form
    {
        FATabStrip CurrentFatrip;
        public Form1()
        {
            InitializeComponent();
        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.faTabTripMaster.RemoveTab(this.faTabTripMaster.SelectedItem);
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            var newtab = new FarsiLibrary.Win.FATabStripItem();
            newtab.Title = Properties.Settings.Default.TAB_TITLE_DEFAULT + "-" + this.faTabTripMaster.Controls.Count;
            var textbox = new DocMapTextBox();
            textbox.Dock = DockStyle.Fill;
            newtab.Controls.Add(textbox);
            this.faTabTripMaster.AddTab(newtab);
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            try
            {
                docMapBox.TextBox.Copy();
            }
            catch (Exception err) { Console.WriteLine(err.Message); }
        }
        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            docMapBox.TextBox.Paste();
        }

        private void redoToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            docMapBox.TextBox.Redo();
        }
        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            docMapBox.TextBox.Undo();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            docMapBox.TextBox.Cut();
        }

        private void faTabTripMaster_TabStripItemSelectionChanged(TabStripItemChangedEventArgs e)
        {
            Console.WriteLine(" lsdlsd");
        }

        private void faTabTripMaster_Click(object sender, EventArgs e)
        {
            // title click
            Console.WriteLine("tabControl title");
        }

        private void DocMapTextBox_Load(object sender, EventArgs e)
        {

        }

        private void faTabTripMaster_Enter(object sender, EventArgs e)
        {
            this.CurrentFatrip = sender as FATabStrip;
        }
    }
}
