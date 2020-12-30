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
        private string pathFolder = "";
        public Form1()
        {
            InitializeComponent();
            LoadXMLScript.LoadFile();
            foreach (string entry in LoadXMLScript.ScriptPathDict.Keys)
            {
                var newitem = new ToolStripMenuItem();
                newitem.Text = entry;
                newitem.Click += languageMenu_Click;
                this.languageToolStripMenuItem.DropDownItems.Add(newitem);
            }
        }
        private DocMapTextBox getCurrentDocMapBox()
        {
            var docMapBox = this.faTabTripMaster.SelectedItem.Controls.Find("DocMapTextBox", false).FirstOrDefault() as DocMapTextBox;
            return docMapBox;
        }
        private void folderView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                e.Node.Nodes.Clear();
                string[] dirs = Directory.GetDirectories(e.Node.Tag.ToString());

                foreach (string dir in dirs)
                {
                    DirectoryInfo di = new DirectoryInfo(dir);
                    TreeNode node = new TreeNode(di.Name, 1, 1);

                    try
                    {
                        node.Tag = dir;
                        if (di.GetDirectories().Count() > 0)
                            node.Nodes.Add(null, "...", 0, 0);
                    }
                    catch (UnauthorizedAccessException)
                    {
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
                        node.Tag = filepath;
                    }
                    catch (UnauthorizedAccessException)
                    {
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
                    pathFolder = folder.SelectedPath;
                    folderView.Nodes.Clear();
                    TreeNode node = new TreeNode(pathFolder, 1, 1);
                    node.Nodes.Add("...");
                    node.Tag = pathFolder;
                    folderView.Nodes.Add(node);
                    node.Expand();
                }
            }


        }
        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Select = this.faTabTripMaster.SelectedItem;
            this.faTabTripMaster.RemoveTab(this.faTabTripMaster.SelectedItem);
            var docMapBox = Select.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            docMapBox.Controls.Clear();
            Select.Dispose();
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            var newtab = new FATabStripItem();
            newtab.Title = Properties.Settings.Default.TAB_TITLE_DEFAULT + "-" + this.faTabTripMaster.Controls.Count;
            var textbox = new DocMapTextBox();
            textbox.Dock = DockStyle.Fill;
            newtab.Controls.Add(textbox);
            textbox.TextBox.TextChanged += this.textboxUpdateInfo_TextChange;
            this.faTabTripMaster.AddTab(newtab);
            this.faTabTripMaster.SelectedItem = newtab;
        }

        private void copyToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            try
            {
                docMapBox.TextBox.Copy();
            }
            catch (Exception err) { Console.WriteLine(err.Message); }
        }
        private void pasteToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.TextBox.Paste();
        }

        private void redoToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.TextBox.Redo();
        }
        private void undoToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.TextBox.Undo();
        }

        private void cutToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.TextBox.Cut();
        }

        private void faTabTripMaster_TabStripItemSelectionChanged(TabStripItemChangedEventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            var ftcb = docMapBox.TextBox;
            this.currentLineCountStatus.Text = "line :" + ftcb.LinesCount.ToString();
            this.currentLanguageStatus.Text = docMapBox.TextBox.Language.ToString();
            this.currentSaveLocationtoolStripStatus.Text = docMapBox.FilePath == "" ? "Never Save Yet" : docMapBox.FilePath;
        }


        private void textboxUpdateInfo_TextChange(object sender, TextChangedEventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
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
                var docMapBox = getCurrentDocMapBox();
                if (docMapBox == null) return;
                var fctb = docMapBox.TextBox;
                fctb.Language = LoadXMLScript.getBuiltInLanguage(currentItem.Text);

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
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.TextBox.SelectAll();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.TextBox.ShowFindDialog();
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.TextBox.ShowReplaceDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void folderView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.folderView.SelectedNode = e.Node;
                var menuscript = new ContextMenuStrip();
                menuscript.Tag = e.Node;
                menuscript.Items.Add("Delete", null);
                //menuscript.Items.Add("Rename", null);
                menuscript.ItemClicked += itemclick_folderView_Click;
                menuscript.Show(folderView, e.X, e.Y);
            }
        }
        private void itemclick_folderView_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            var cms = sender as ContextMenuStrip;
            var node = cms.Tag as TreeNode;
            var path = node.FullPath;
            try
            {
                if (DirAndFileExt.isDirectory(path))
                {
                    //dicrectory
                    switch (e.ClickedItem.Text)
                    {
                        case "Delete":
                            Directory.Delete(path);
                            node.Remove();
                            break;
                        case "Rename":
                            if (node != node.FirstNode && !node.IsEditing)
                            {
                                node.BeginEdit();
                            }
                            else
                            {
                                MessageBox.Show("Cant edit folder was open");
                            }
                            break;
                    }
                }
                else
                {
                    switch (e.ClickedItem.Text)
                    {
                        case "Delete":
                            File.Delete(path);
                            node.Remove();
                            break;
                        case "Rename":
                            if (node != node.FirstNode && !node.IsEditing)
                            {
                                node.BeginEdit();
                            }
                            else
                            {
                                MessageBox.Show("Cant edit folder was open");
                            }
                            break;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newToolStripButton.PerformClick();
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.OpenFile();
            if (docMapBox.FilePath == "")
            {
                closeTabToolStripMenuItem.PerformClick();
            }
            else
            {
                this.faTabTripMaster.SelectedItem.Title = Path.GetFileName(docMapBox.FilePath);
                this.currentSaveLocationtoolStripStatus.Text = docMapBox.FilePath;
                languageToolStripMenuItem.DropDownItems
                       .OfType<ToolStripMenuItem>().ToList()
                       .ForEach(item =>
                       {
                           if (LoadXMLScript.getBuiltInLanguage(item.Text) == docMapBox.TextBox.Language)
                           {
                               item.Checked = true;
                           }
                           else
                           {
                               item.Checked = false;
                           }
                       });
            }
        }
        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.SaveFile();
            this.currentSaveLocationtoolStripStatus.Text = docMapBox.FilePath == "" ? "Never Save Yet" : docMapBox.FilePath;
            if (docMapBox.FilePath != "")
            {
                this.faTabTripMaster.SelectedItem.Title = Path.GetFileName(docMapBox.FilePath);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.SaveAsFile();
            this.currentSaveLocationtoolStripStatus.Text = docMapBox.FilePath == "" ? "Never Save Yet" : docMapBox.FilePath;
        }
        private void runCS_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            var path = docMapBox.FilePath;
            if (path == "")
            {
                string dir = Path.GetTempPath() + @"\NotepadDIY";
                Directory.CreateDirectory(dir);
                path = dir + @"\tempfile.cs";
                docMapBox.TextBox.SaveToFile(path, Encoding.Default);
            }
            else
            {
                path = path.SanitizePath('_');
                docMapBox.TextBox.SaveToFile(path, Encoding.Default);
            }
            FileInfo fileInfo = new FileInfo(path);
            docMapBox.TextBox.SaveToFile(path, Encoding.Default);
            debugControl1.compileRunCSharp(docMapBox.TextBox.Text, fileInfo);
        }
        private void runCPP_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            var path = docMapBox.FilePath;
            if (path == "")
            {
                string dir = Path.GetTempPath() + @"\NotepadDIY";
                Directory.CreateDirectory(dir);
                path = dir + @"\tempfile.cpp";
                docMapBox.TextBox.SaveToFile(path, Encoding.Default);
            }
            else
            {
                path = path.SanitizePath('_');
                docMapBox.TextBox.SaveToFile(path, Encoding.Default);
            }
            FileInfo fileInfo = new FileInfo(path);
            debugControl1.compileRunCPP(fileInfo);
        }
        private void Javascript_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            var path = docMapBox.FilePath;
            if (path == "")
            {
                string dir = Path.GetTempPath() + @"\NotepadDIY";
                Directory.CreateDirectory(dir);
                path = dir + @"\tempfile.js";
                docMapBox.TextBox.SaveToFile(path, Encoding.Default);
            }
            else
            {
                path = path.SanitizePath('_');
                docMapBox.TextBox.SaveToFile(path, Encoding.Default);
            }
            FileInfo fileInfo = new FileInfo(path);
            debugControl1.compileRunNodejs(fileInfo);
        }

        private void folderView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                FileAttributes attr = File.GetAttributes(e.Node.FullPath);
                if (attr.HasFlag(FileAttributes.Directory))
                {
                    e.Node.Expand();
                }
                else
                {
                    newToolStripButton.PerformClick();
                    var docMapBox = getCurrentDocMapBox();
                    if (docMapBox == null) return;
                    docMapBox.LoadFile(e.Node.FullPath);
                    this.faTabTripMaster.SelectedItem.Title = Path.GetFileName(docMapBox.FilePath);
                    this.currentSaveLocationtoolStripStatus.Text = docMapBox.FilePath;
                    languageToolStripMenuItem.DropDownItems
                           .OfType<ToolStripMenuItem>().ToList()
                           .ForEach(item =>
                           {
                               if (LoadXMLScript.getBuiltInLanguage(item.Text) == docMapBox.TextBox.Language)
                               {
                                   item.Checked = true;
                               }
                               else
                               {
                                   item.Checked = false;
                               }
                           });
                }
            }
        }

        private void faTabTripMaster_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                FATabStripItem tab = this.faTabTripMaster.GetTabItemByPoint(e.Location);
                if (tab is null) return;
                var menuscript = new ContextMenuStrip();
                menuscript.Tag = tab;
                menuscript.Items.Add("New Tab");
                menuscript.Items.Add("Close");
                menuscript.Items.Add(new ToolStripSeparator());
                menuscript.Items.Add("Move to First", null);
                menuscript.Items.Add("Move to Last", null);
                menuscript.Items.Add(new ToolStripSeparator());
                menuscript.Items.Add("Close All Tab", null);
                menuscript.Items.Add("Close All Tab But This", null);
                menuscript.ItemClicked += item_faTabTripMaster_RightClick;
                // + groupbox1 width because FAtabtrip bug
                menuscript.Show(folderView, e.X + this.groupBox1.Width, e.Y);
            }
        }
        private void item_faTabTripMaster_RightClick(object sender, ToolStripItemClickedEventArgs e)
        {

            var cms = sender as ContextMenuStrip;
            var tab = cms.Tag as FATabStripItem;
            switch (e.ClickedItem.Text)
            {
                case "New Tab":
                    newToolStripButton.PerformClick();
                    break;
                case "Close":
                    this.faTabTripMaster.RemoveTab(tab);
                    break;
                case "Move to First":
                    this.faTabTripMaster.Items.MoveTo(0, tab);
                    break;
                case "Move to Last":
                    this.faTabTripMaster.Items.MoveTo(this.faTabTripMaster.Items.Count - 1, tab);
                    break;
                case "Close All Tab":
                    this.faTabTripMaster.Items.Clear();
                    break;
                case "Close All Tab But This":
                    this.faTabTripMaster.Items.Clear();
                    this.faTabTripMaster.Items.Add(tab);
                    this.faTabTripMaster.SelectedItem = tab;
                    break;
            }
        }
        string label_beforePath_Node;
        private void folderView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            try
            {
                if (DirAndFileExt.isDirectory(label_beforePath_Node))
                {
                    DirAndFileExt.RenameFolder(label_beforePath_Node, e.Label);
                }
                else
                {
                    DirAndFileExt.RenameFile(label_beforePath_Node, e.Label);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void folderView_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            label_beforePath_Node = e.Node.FullPath;
        }
    }
}