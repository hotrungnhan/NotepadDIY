
namespace NotepadDIY.Components
{
    partial class CompileRun
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.runTabStrip = new FarsiLibrary.Win.FATabStrip();
            this.errorLog = new FarsiLibrary.Win.FATabStripItem();
            this.termimalLog = new FarsiLibrary.Win.FATabStripItem();
            this.errorTextBox = new System.Windows.Forms.RichTextBox();
            this.terminalTextBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.runTabStrip)).BeginInit();
            this.runTabStrip.SuspendLayout();
            this.errorLog.SuspendLayout();
            this.termimalLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // runTabStrip
            // 
            this.runTabStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runTabStrip.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.runTabStrip.Items.AddRange(new FarsiLibrary.Win.FATabStripItem[] {
            this.errorLog,
            this.termimalLog});
            this.runTabStrip.Location = new System.Drawing.Point(0, 0);
            this.runTabStrip.Name = "runTabStrip";
            this.runTabStrip.SelectedItem = this.errorLog;
            this.runTabStrip.Size = new System.Drawing.Size(594, 194);
            this.runTabStrip.TabIndex = 0;
            this.runTabStrip.Text = "runTabStrip";
            // 
            // errorLog
            // 
            this.errorLog.Controls.Add(this.errorTextBox);
            this.errorLog.IsDrawn = true;
            this.errorLog.Name = "errorLog";
            this.errorLog.Selected = true;
            this.errorLog.Size = new System.Drawing.Size(592, 173);
            this.errorLog.TabIndex = 0;
            this.errorLog.Title = "Error Log";
            // 
            // termimalLog
            // 
            this.termimalLog.Controls.Add(this.terminalTextBox);
            this.termimalLog.IsDrawn = true;
            this.termimalLog.Name = "termimalLog";
            this.termimalLog.Size = new System.Drawing.Size(592, 173);
            this.termimalLog.TabIndex = 1;
            this.termimalLog.Title = "Terminal";
            // 
            // errorTextBox
            // 
            this.errorTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorTextBox.Location = new System.Drawing.Point(0, 0);
            this.errorTextBox.Name = "errorTextBox";
            this.errorTextBox.Size = new System.Drawing.Size(592, 173);
            this.errorTextBox.TabIndex = 0;
            this.errorTextBox.Text = "";
            // 
            // terminalTextBox
            // 
            this.terminalTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.terminalTextBox.Location = new System.Drawing.Point(0, 0);
            this.terminalTextBox.Name = "terminalTextBox";
            this.terminalTextBox.Size = new System.Drawing.Size(592, 173);
            this.terminalTextBox.TabIndex = 0;
            this.terminalTextBox.Text = "";
            // 
            // CompileRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.runTabStrip);
            this.Name = "CompileRun";
            this.Size = new System.Drawing.Size(594, 194);
            ((System.ComponentModel.ISupportInitialize)(this.runTabStrip)).EndInit();
            this.runTabStrip.ResumeLayout(false);
            this.errorLog.ResumeLayout(false);
            this.termimalLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FarsiLibrary.Win.FATabStrip runTabStrip;
        private FarsiLibrary.Win.FATabStripItem errorLog;
        private System.Windows.Forms.RichTextBox errorTextBox;
        private FarsiLibrary.Win.FATabStripItem termimalLog;
        private System.Windows.Forms.RichTextBox terminalTextBox;
    }
}
