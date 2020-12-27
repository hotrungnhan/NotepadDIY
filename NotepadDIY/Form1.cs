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
        
        FATabStrip CurrentFatrip;
        
        public Form1()
        {
            InitializeComponent();
            LoadXMLScript.LoadFile();
            foreach (string entry in LoadXMLScript.ScriptPathDict.Keys)
            {
                var newitem = new ToolStripMenuItem();
                newitem.Text = entry;
                this.languageToolStripMenuItem.DropDownItems.Add(newitem);
            }


        }
        private bool CheckExist(string name)
        {
            if (ListCurrentTab.Contains(name))
            {
                return true;
            }
            return false;
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
                        //if (di.GetDirectories().Count() > 0)
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
                    folderView.Nodes.Clear();
                    TreeNode node = new TreeNode(path, 1, 1);
                    node.Nodes.Add("...");
                    node.Tag = path;
                    folderView.Nodes.Add(node);
                }
            }


        }
        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var Select = this.faTabTripMaster.SelectedItem;
            if (CheckExist(Select.Tag.ToString()))
            {
                ListCurrentTab.Remove(Select.Tag.ToString());
            }
            this.faTabTripMaster.RemoveTab(this.faTabTripMaster.SelectedItem);
            var docMapBox = Select.Controls.Find("DocMapTextBox", true).First() as DocMapTextBox;
            docMapBox.Controls.Clear();
            Select.Dispose();
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

        private void faTabTripMaster_Click(object sender, EventArgs e)
        {
            // title click
            Console.WriteLine("tabControl title");
        }

        private void faTabTripMaster_Enter(object sender, EventArgs e)
        {
            this.CurrentFatrip = sender as FATabStrip;
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
            if (e.Button == MouseButtons.Right && e.Node.SelectedImageIndex == 1)
            {
                contextMS_RightClick_Folder.Show(folderView,e.X,e.Y);
                currentNode = e.Node;
            }
            if (e.Button == MouseButtons.Right && e.Node.SelectedImageIndex == 0)
            {
                currentNode = e.Node;
                contextMS_RightClick_File.Show(folderView, e.X, e.Y);
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var opendialog = new OpenFileDialog();
            if (opendialog.ShowDialog() == DialogResult.OK)
            {
                newToolStripButton.PerformClick();
                var docMapBox = getCurrentDocMapBox();
                if (docMapBox == null) return;
                docMapBox.LoadFile(opendialog.FileName);
                this.faTabTripMaster.SelectedItem.Title = Path.GetFileName(opendialog.FileName);
                this.currentSaveLocationtoolStripStatus.Text = docMapBox.FilePath == "" ? "Never Save Yet" : docMapBox.FilePath;
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.SaveFile();
            this.currentSaveLocationtoolStripStatus.Text = docMapBox.FilePath == "" ? "Never Save Yet" : docMapBox.FilePath;
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var docMapBox = getCurrentDocMapBox();
            if (docMapBox == null) return;
            docMapBox.SaveAsFile();
            this.currentSaveLocationtoolStripStatus.Text = docMapBox.FilePath == "" ? "Never Save Yet" : docMapBox.FilePath;
        }


        #region contextmenustrip
        private string path = "";
        TreeNode NodeCutCopy = null;
        bool isCopyD = false;
        bool isCutD = false;
        bool isCopyF = false;
        bool isCutF = false;
        private string SourcePath = "";
        TreeNode currentNode = null;
        List<string> ListCurrentTab = new List<string>();
        private void contextFolder_AddItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                using (AddItem add =new AddItem())
                {
                    if (add.ShowDialog() == DialogResult.OK)
                    {
                        TreeNode node = new TreeNode(add.nameFile, 0, 0);
                        string pathString = System.IO.Path.Combine(currentNode.Tag.ToString(), add.nameFile);
                        if (System.IO.File.Exists(pathString))
                        {
                            MessageBox.Show("File name exists");
                            return;
                        }
                        File.WriteAllText(pathString, "");
                        node.Tag = pathString;
                        currentNode.Nodes.Add(node);
                    }
                }
            }
        }

        private void contextFolder_DeleteItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                for (int i = 0; i < ListCurrentTab.Count; i++)
                {
                    if (ListCurrentTab[i].IndexOf(currentNode.Tag.ToString()) != -1)
                    {
                        var Select = this.faTabTripMaster.Items[ListCurrentTab.Count - 1 - i];
                        this.faTabTripMaster.RemoveTab(Select);
                        ListCurrentTab.Remove(ListCurrentTab[i]);
                    }
                }
                currentNode.Remove();
            }
        }

        private void contextfile_OpenItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                if (CheckExist(currentNode.Tag.ToString() + " " + currentNode.Text))
                {
                    this.faTabTripMaster.SelectedItem = this.faTabTripMaster.Items[ListCurrentTab.Count - 1 - ListCurrentTab.IndexOf(currentNode.Tag.ToString() +" "+ currentNode.Text)];
                    return;
                }
                else
                {
                    ListCurrentTab.Add(currentNode.Tag.ToString() + " " + currentNode.Text);
                    var newtab = new FarsiLibrary.Win.FATabStripItem();
                    newtab.Title = currentNode.Text;
                    newtab.Tag = currentNode.Tag.ToString() + " " + currentNode.Text;
                    var textbox = new DocMapTextBox();
                    textbox.Dock = DockStyle.Fill;
                    textbox.TextBox.Text = File.ReadAllText(currentNode.Tag.ToString());
                    newtab.Controls.Add(textbox);
                    textbox.TextBox.TextChanged += this.textboxUpdateInfo_TextChange;
                    this.faTabTripMaster.AddTab(newtab);
                    this.faTabTripMaster.SelectedItem = this.faTabTripMaster.Items[this.faTabTripMaster.Items.IndexOf(newtab)];
                }
            }
        }

        private void contextfile_DeleteItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                var Select = this.faTabTripMaster.Items[ListCurrentTab.Count - 1 - ListCurrentTab.IndexOf(currentNode.Tag.ToString() + " " + currentNode.Text)]; 
                if (CheckExist(currentNode.Tag.ToString() + " " + Select.Title))
                {
                    ListCurrentTab.Remove(currentNode.Tag.ToString() + " " + Select.Title);
                }
                this.faTabTripMaster.RemoveTab(Select);
                currentNode.Remove();
            }
        }

        private void folderView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.SelectedImageIndex == 0)
            {
                currentNode = e.Node;
                if (CheckExist(currentNode.Tag.ToString() + " " + currentNode.Text))
                {
                    this.faTabTripMaster.SelectedItem = this.faTabTripMaster.Items[ListCurrentTab.Count - 1 - ListCurrentTab.IndexOf(currentNode.Tag.ToString() + " " + currentNode.Text)];
                    return;
                }
                ListCurrentTab.Add(currentNode.Tag.ToString() + " " + currentNode.Text);
                var newtab = new FarsiLibrary.Win.FATabStripItem();
                newtab.Title = currentNode.Text;
                newtab.Tag = currentNode.Tag.ToString() + " " + currentNode.Text;
                var textbox = new DocMapTextBox();
                textbox.Dock = DockStyle.Fill;
                textbox.TextBox.Text = File.ReadAllText(currentNode.Tag.ToString());
                newtab.Controls.Add(textbox);
                textbox.TextBox.TextChanged += this.textboxUpdateInfo_TextChange;
                this.faTabTripMaster.AddTab(newtab);
                this.faTabTripMaster.SelectedItem = this.faTabTripMaster.Items[this.faTabTripMaster.Items.IndexOf(newtab)];
            }
        }

        private void contextFolder_AddFolerItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                using (AddItem add = new AddItem())
                {
                    if (add.ShowDialog() == DialogResult.OK)
                    {
                        TreeNode node = new TreeNode(add.nameFile, 1, 1);
                        string pathString = System.IO.Path.Combine(currentNode.Tag.ToString(), add.nameFile);
                        if (System.IO.Directory.Exists(pathString))
                        {
                            MessageBox.Show("Folder name exist");
                            return;
                        }
                        System.IO.Directory.CreateDirectory(pathString);
                        node.Tag = pathString;
                        currentNode.Nodes.Add(node);
                    }
                }
            }
        }
        private void contextfolder_CopyItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                NodeCutCopy = new TreeNode(currentNode.Text,1,1);
                SourcePath = currentNode.Tag.ToString();
                isCopyD = true;
            }
        }
        private void contextFolder_CutItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                NodeCutCopy = new TreeNode(currentNode.Text,1,1);
                SourcePath = currentNode.Tag.ToString();
                isCutD = true;
                currentNode.Remove();
            }
        }
        private void contextFolder_PasteItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null && NodeCutCopy != null)
            {
                if (isCopyD)
                {
                    string dest = Path.Combine(currentNode.Tag.ToString(), NodeCutCopy.Text);
                    Directory.CreateDirectory(dest);
                    if (System.IO.Directory.Exists(SourcePath))
                    {
                        string[] files = System.IO.Directory.GetFiles(SourcePath);

                        // Copy the files and overwrite destination files if they already exist.
                        foreach (string s in files)
                        {
                            // Use static Path methods to extract only the file name from the path.
                            string fileName = System.IO.Path.GetFileName(s);
                            string destfile = System.IO.Path.Combine(dest, fileName);
                            System.IO.File.Copy(s, destfile, true);
                        }
                        NodeCutCopy.Tag = dest;
                        currentNode.Nodes.Add(NodeCutCopy);
                        NodeCutCopy = null;
                        isCopyD = false;
                        SourcePath = "";
                    }
                    else
                    {
                        Console.WriteLine("Source path does not exist!");
                        NodeCutCopy = null;
                        isCutD = false;
                        SourcePath = "";
                    }
                }
                if (isCutD)
                {
                    string dest = Path.Combine(currentNode.Tag.ToString(), NodeCutCopy.Text);
                    if (System.IO.Directory.Exists(SourcePath))
                    {
                        Directory.Move(SourcePath, dest);
                        NodeCutCopy.Tag = dest;
                        currentNode.Nodes.Add(NodeCutCopy);
                        NodeCutCopy = null;
                        isCutD= false;
                        SourcePath = "";
                    }
                    else
                    {
                        Console.WriteLine("Source path does not exist!");
                        NodeCutCopy = null;
                        isCutD = false;
                        SourcePath = "";
                    }
                }
                if (isCopyF)
                {
                    string dest = Path.Combine(currentNode.Tag.ToString(), NodeCutCopy.Text);
                    if (System.IO.File.Exists(SourcePath))
                    {
                        MessageBox.Show("DA VAO DAY");
                        File.Copy(SourcePath, dest);
                        NodeCutCopy.Tag = dest;
                        currentNode.Nodes.Add(NodeCutCopy);
                        NodeCutCopy = null;
                        isCopyF = false;
                        SourcePath = "";
                    }
                    else
                    {
                        Console.WriteLine("Source path does not exist!");
                        NodeCutCopy = null;
                        isCopyF = false;
                        SourcePath = "";
                    }
                }
                if (isCutF)
                {
                    string dest = Path.Combine(currentNode.Tag.ToString(), NodeCutCopy.Text);
                    if (System.IO.File.Exists(SourcePath))
                    {
                        MessageBox.Show("DA VAO DAY");
                        File.Move(SourcePath, dest);
                        NodeCutCopy.Tag = dest;
                        currentNode.Nodes.Add(NodeCutCopy);
                        NodeCutCopy = null;
                        isCutF = false;
                        SourcePath = "";
                    }
                    else
                    {
                        Console.WriteLine("Source path does not exist!");
                        NodeCutCopy = null;
                        isCopyF = false;
                        SourcePath = "";
                    }
                }

            }
            
        }
        private void contextFile_CopyItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                MessageBox.Show("VAo DAY roi");
                NodeCutCopy = new TreeNode(currentNode.Text, 0, 0);
                SourcePath = currentNode.Tag.ToString();
                isCopyF = true;
            }
        }

        private void contextFile_CutItem_Click(object sender, EventArgs e)
        {
            if (currentNode != null)
            {
                MessageBox.Show("VAo DAY roi");
                NodeCutCopy = new TreeNode(currentNode.Text, 0, 0);
                SourcePath = currentNode.Tag.ToString();
                isCutF = true;
                currentNode.Remove();
            }
        }

        #endregion


    }
}
