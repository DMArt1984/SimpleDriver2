
namespace WinSimpleIDriver.Editor
{
    partial class FormDesign
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadBacgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelType = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripMenuItemAddControl = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddControlLabel = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddControlOutput = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAddControlPicture = new System.Windows.Forms.ToolStripMenuItem();
            this.командыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCommandCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemCommandDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveJsonToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.loadBacgroundToolStripMenuItem,
            this.ToolStripMenuItemAddControl,
            this.командыToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(659, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadBacgroundToolStripMenuItem
            // 
            this.loadBacgroundToolStripMenuItem.Name = "loadBacgroundToolStripMenuItem";
            this.loadBacgroundToolStripMenuItem.Size = new System.Drawing.Size(103, 20);
            this.loadBacgroundToolStripMenuItem.Text = "load bacground";
            this.loadBacgroundToolStripMenuItem.Click += new System.EventHandler(this.loadBacgroundToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelType,
            this.toolStripStatusLabelTitle});
            this.statusStrip1.Location = new System.Drawing.Point(0, 360);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(659, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelType
            // 
            this.toolStripStatusLabelType.Name = "toolStripStatusLabelType";
            this.toolStripStatusLabelType.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabelType.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabelTitle
            // 
            this.toolStripStatusLabelTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolStripStatusLabelTitle.Name = "toolStripStatusLabelTitle";
            this.toolStripStatusLabelTitle.Size = new System.Drawing.Size(127, 17);
            this.toolStripStatusLabelTitle.Text = "toolStripStatusLabel2";
            // 
            // ToolStripMenuItemAddControl
            // 
            this.ToolStripMenuItemAddControl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAddControlLabel,
            this.ToolStripMenuItemAddControlOutput,
            this.ToolStripMenuItemAddControlPicture});
            this.ToolStripMenuItemAddControl.Name = "ToolStripMenuItemAddControl";
            this.ToolStripMenuItemAddControl.Size = new System.Drawing.Size(71, 20);
            this.ToolStripMenuItemAddControl.Text = "Добавить";
            // 
            // ToolStripMenuItemAddControlLabel
            // 
            this.ToolStripMenuItemAddControlLabel.Name = "ToolStripMenuItemAddControlLabel";
            this.ToolStripMenuItemAddControlLabel.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemAddControlLabel.Text = "Label";
            this.ToolStripMenuItemAddControlLabel.Click += new System.EventHandler(this.ToolStripMenuItemAddControlLabel_Click);
            // 
            // ToolStripMenuItemAddControlOutput
            // 
            this.ToolStripMenuItemAddControlOutput.Name = "ToolStripMenuItemAddControlOutput";
            this.ToolStripMenuItemAddControlOutput.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemAddControlOutput.Text = "OutputBox";
            this.ToolStripMenuItemAddControlOutput.Click += new System.EventHandler(this.ToolStripMenuItemAddControlOutput_Click);
            // 
            // ToolStripMenuItemAddControlPicture
            // 
            this.ToolStripMenuItemAddControlPicture.Name = "ToolStripMenuItemAddControlPicture";
            this.ToolStripMenuItemAddControlPicture.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemAddControlPicture.Text = "Picture";
            this.ToolStripMenuItemAddControlPicture.Click += new System.EventHandler(this.ToolStripMenuItemAddControlPicture_Click);
            // 
            // командыToolStripMenuItem
            // 
            this.командыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemCommandCopy,
            this.ToolStripMenuItemCommandDelete});
            this.командыToolStripMenuItem.Name = "командыToolStripMenuItem";
            this.командыToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.командыToolStripMenuItem.Text = "Команды";
            // 
            // ToolStripMenuItemCommandCopy
            // 
            this.ToolStripMenuItemCommandCopy.Name = "ToolStripMenuItemCommandCopy";
            this.ToolStripMenuItemCommandCopy.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemCommandCopy.Text = "Дубликат";
            this.ToolStripMenuItemCommandCopy.Click += new System.EventHandler(this.ToolStripMenuItemCommandCopy_Click);
            // 
            // ToolStripMenuItemCommandDelete
            // 
            this.ToolStripMenuItemCommandDelete.Name = "ToolStripMenuItemCommandDelete";
            this.ToolStripMenuItemCommandDelete.Size = new System.Drawing.Size(180, 22);
            this.ToolStripMenuItemCommandDelete.Text = "Удалить";
            this.ToolStripMenuItemCommandDelete.Click += new System.EventHandler(this.ToolStripMenuItemCommandDelete_Click);
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveJsonToolStripMenuItem1});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "load json";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveJsonToolStripMenuItem1
            // 
            this.saveJsonToolStripMenuItem1.Name = "saveJsonToolStripMenuItem1";
            this.saveJsonToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            this.saveJsonToolStripMenuItem1.Text = "save json";
            this.saveJsonToolStripMenuItem1.Click += new System.EventHandler(this.saveJsonToolStripMenuItem1_Click);
            // 
            // FormDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 382);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormDesign";
            this.Text = "FormDesign";
            this.Load += new System.EventHandler(this.FormDesign_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadBacgroundToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelType;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelTitle;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddControl;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddControlLabel;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddControlOutput;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAddControlPicture;
        private System.Windows.Forms.ToolStripMenuItem командыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCommandCopy;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemCommandDelete;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveJsonToolStripMenuItem1;
    }
}