using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NotepadDIY.Components;
using NotepadDIY.Properties;
using FarsiLibrary.Win;
using FastColoredTextBoxNS;
namespace NotepadDIY
{

    public partial class Form1 : Form
    {
        private string path = "";
        FATabStrip CurrentFatrip;
        public Form1()
        {
            InitializeComponent();
        }
        private void folderView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                e.Node.Nodes.Clear();
                //get the list of sub direcotires
                string[] dirs = Directory.GetDirectories(e.Node.Tag.ToString());

                foreach (string dir in dirs)
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    TreeNode node = new TreeNode(di.Name, 1, 1);
                    try
                    {
                        //keep the directory's full path in the tag for use later
                        node.Tag = dir;

                        // if the directory has sub directories add the place holder
                        if (di.GetDirectories().Count() > 0)
                            node.Nodes.Add(null, "...", 0, 0);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        //display a locked folder icon
                        node.ImageIndex = 0;
                        node.SelectedImageIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "DirectoryLister",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        e.Node.Nodes.Add(node);
                    }
                }
                string[] filePath = Directory.GetFiles(e.Node.Tag.ToString());
                foreach (string filepath in filePath)
                {
                    FileInfo file = new FileInfo(filepath);
                    TreeNode node = new TreeNode(file.Name, 0, 0);
                    try
                    {
                        //keep the directory's full path in the tag for use later
                        node.Tag = filepath;
                        // if the directory has sub directories add the place holder
                    }
                    catch (UnauthorizedAccessException)
                    {
                        //display a locked folder icon
                        node.ImageIndex = 0;
                        node.SelectedImageIndex = 0;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "DirectoryLister",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        e.Node.Nodes.Add(node);
                    }
                }

            }
        }

        private void folderView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 1)
            {
                e.Node.Nodes.Clear();
                e.Node.Nodes.Add(null, "...", 0, 0);
            }
        }

        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folder = new FolderBrowserDialog())
            {
                if (folder.ShowDialog() == DialogResult.OK)
                {
                    path = folder.SelectedPath;
                }
            }
            folderView.Nodes.Clear();
            TreeNode node = new TreeNode(path, 1, 1);
            node.Nodes.Add("...");
            node.Tag = path;
            folderView.Nodes.Add(node);
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
            textbox.TextBox.TextChanged += this.textboxUpdateInfo_TextChange;
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
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            var ftcb = docMapBox.TextBox;
            this.currentLineCountStatus.Text = "line :" + ftcb.LinesCount.ToString();
            this.currentLanguageStatus.Text = docMapBox.TextBox.Language.ToString();
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
        private void textboxUpdateInfo_TextChange(object sender, TextChangedEventArgs e)
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            var ftcb = docMapBox.TextBox;
            this.currentLineCountStatus.Text = "line :" + ftcb.LinesCount.ToString();
        }
        private void languageMenu_Click(object sender, EventArgs e)
        {
            var currentItem = sender as ToolStripMenuItem;
            if (currentItem != null)
            {
                ((ToolStripMenuItem)currentItem.OwnerItem).DropDownItems
                    .OfType<ToolStripMenuItem>().ToList()
                    .ForEach(item =>
                    {
                        item.Checked = false;
                    });
                // set tab Lang item;
                var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
                var fctb = docMapBox.TextBox;
                switch (currentItem.Text)
                {
                    //For example, we will highlight the syntax of C# manually, although could use built-in highlighter
                    case "Plain":
                        fctb.Language = Language.Custom;
                        break;
                    case "CSharp": fctb.Language = Language.CSharp; break;
                    case "VB": fctb.Language = Language.VB; break;
                    case "HTML": fctb.Language = Language.HTML; break;
                    case "XML": fctb.Language = Language.XML; break;
                    case "SQL": fctb.Language = Language.SQL; break;
                    case "PHP": fctb.Language = Language.PHP; break;
                    case "JS": fctb.Language = Language.JS; break;
                    case "Lua": fctb.Language = Language.Lua; break;
                    case "JSON": fctb.Language = Language.JSON; break;
                }
                //Check the current items
                currentItem.Checked = true;
                fctb.OnSyntaxHighlight(new TextChangedEventArgs(fctb.Range));
                this.currentLanguageStatus.Text = currentItem.Text;
            }
        }

        private void rulerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rulerToolStripMenuItem.Checked = !rulerToolStripMenuItem.Checked;
            foreach (FATabStripItem item in this.faTabTripMaster.Items)
            {
                var docMapBox = item.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
                docMapBox.RulerEnable = rulerToolStripMenuItem.Checked;
            }

        }

        private void splitToolStripButton_Click(object sender, EventArgs e)
        {
            splitToolStripButton.Checked = !splitToolStripButton.Checked;
            if (splitToolStripButton.Checked)
            {
                splitContainer2.SplitterDistance = (int)(splitContainer2.Size.Width * 0.5);
                splitContainer2.Panel2Collapsed = false;

            }
            else
            {
                splitContainer2.Panel2Collapsed = true;
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            docMapBox.TextBox.SelectAll();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            docMapBox.TextBox.ShowFindDialog();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            docMapBox.TextBox.ShowReplaceDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
