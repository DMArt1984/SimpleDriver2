
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
            this.addLabelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addOutputboxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveJsonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPictureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadBacgroundToolStripMenuItem,
            this.addLabelToolStripMenuItem,
            this.addOutputboxToolStripMenuItem,
            this.addPictureToolStripMenuItem,
            this.saveJsonToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
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
            // addLabelToolStripMenuItem
            // 
            this.addLabelToolStripMenuItem.Name = "addLabelToolStripMenuItem";
            this.addLabelToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.addLabelToolStripMenuItem.Text = "add label";
            this.addLabelToolStripMenuItem.Click += new System.EventHandler(this.addLabelToolStripMenuItem_Click);
            // 
            // addOutputboxToolStripMenuItem
            // 
            this.addOutputboxToolStripMenuItem.Name = "addOutputboxToolStripMenuItem";
            this.addOutputboxToolStripMenuItem.Size = new System.Drawing.Size(97, 20);
            this.addOutputboxToolStripMenuItem.Text = "add outputbox";
            this.addOutputboxToolStripMenuItem.Click += new System.EventHandler(this.addOutputboxToolStripMenuItem_Click);
            // 
            // saveJsonToolStripMenuItem
            // 
            this.saveJsonToolStripMenuItem.Name = "saveJsonToolStripMenuItem";
            this.saveJsonToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.saveJsonToolStripMenuItem.Text = "save json";
            this.saveJsonToolStripMenuItem.Click += new System.EventHandler(this.saveJsonToolStripMenuItem_Click);
            // 
            // addPictureToolStripMenuItem
            // 
            this.addPictureToolStripMenuItem.Name = "addPictureToolStripMenuItem";
            this.addPictureToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.addPictureToolStripMenuItem.Text = "add picture";
            this.addPictureToolStripMenuItem.Click += new System.EventHandler(this.addPictureToolStripMenuItem_Click);
            // 
            // FormDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormDesign";
            this.Text = "FormDesign";
            this.Load += new System.EventHandler(this.FormDesign_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadBacgroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addLabelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addOutputboxToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveJsonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPictureToolStripMenuItem;
    }
}