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
using FastColoredTextBoxNS;
using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;


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

        private void runCSharp_Click(object sender, EventArgs e)
        {
            //CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            //ICodeCompiler icc = codeProvider.CreateCompiler();
            //string Output = "Out.exe";
            
            
            
            //System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            ////Make sure we generate an EXE, not a DLL
            //parameters.GenerateExecutable = true;
            //parameters.OutputAssembly = Output;
            //CompilerResults results = icc.CompileAssemblyFromSource(parameters, this.docMapTextBox.GetText());

            //if (results.Errors.Count > 0)
            //{
            //    textBox2.ForeColor = Color.Red;
            //    foreach (CompilerError CompErr in results.Errors)
            //    {
            //        textBox2.Text = textBox2.Text +
            //                    "Line number " + CompErr.Line +
            //                    ", Error Number: " + CompErr.ErrorNumber +
            //                    ", '" + CompErr.ErrorText + ";" +
            //                    Environment.NewLine + Environment.NewLine;
            //    }
            //}
            //else
            //{
            //    //Successful Compile
            //    textBox2.ForeColor = Color.Blue;
            //    textBox2.Text = "Success!";
            //}
        }
    }
}
