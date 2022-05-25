namespace BmlExtract
{
    partial class Form1
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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressPrsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decompressPrsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bigEndianCheck = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.prsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(229, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.packToolStripMenuItem,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.openToolStripMenuItem.Text = "Extract";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // packToolStripMenuItem
            // 
            this.packToolStripMenuItem.Name = "packToolStripMenuItem";
            this.packToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.packToolStripMenuItem.Text = "Pack";
            this.packToolStripMenuItem.Click += new System.EventHandler(this.packToolStripMenuItem_Click);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // prsToolStripMenuItem
            // 
            this.prsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compressPrsToolStripMenuItem,
            this.decompressPrsToolStripMenuItem});
            this.prsToolStripMenuItem.Name = "prsToolStripMenuItem";
            this.prsToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.prsToolStripMenuItem.Text = "Prs";
            // 
            // compressPrsToolStripMenuItem
            // 
            this.compressPrsToolStripMenuItem.Name = "compressPrsToolStripMenuItem";
            this.compressPrsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.compressPrsToolStripMenuItem.Text = "Compress Prs";
            this.compressPrsToolStripMenuItem.Click += new System.EventHandler(this.compressPrsToolStripMenuItem_Click);
            // 
            // decompressPrsToolStripMenuItem
            // 
            this.decompressPrsToolStripMenuItem.Name = "decompressPrsToolStripMenuItem";
            this.decompressPrsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.decompressPrsToolStripMenuItem.Text = "Decompress Prs";
            this.decompressPrsToolStripMenuItem.Click += new System.EventHandler(this.decompressPrsToolStripMenuItem_Click);
            // 
            // bigEndianCheck
            // 
            this.bigEndianCheck.AutoSize = true;
            this.bigEndianCheck.Location = new System.Drawing.Point(14, 53);
            this.bigEndianCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bigEndianCheck.Name = "bigEndianCheck";
            this.bigEndianCheck.Size = new System.Drawing.Size(192, 19);
            this.bigEndianCheck.TabIndex = 1;
            this.bigEndianCheck.Text = "Pack to Big Endian (Gamecube)";
            this.bigEndianCheck.UseVisualStyleBackColor = true;
            this.bigEndianCheck.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(10, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Packing Options";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 113);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.bigEndianCheck);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = ".bml Handler";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packToolStripMenuItem;
        private System.Windows.Forms.CheckBox bigEndianCheck;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem prsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compressPrsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decompressPrsToolStripMenuItem;
    }
}

