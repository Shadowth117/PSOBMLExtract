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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            packToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            packTogslToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            prsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            compressPrsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            decompressPrsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            bigEndianCheck = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            recursiveUnpackCB = new System.Windows.Forms.CheckBox();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, prsToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip1.Size = new System.Drawing.Size(249, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, packToolStripMenuItem, packTogslToolStripMenuItem, quitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            openToolStripMenuItem.Text = "Extract";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // packToolStripMenuItem
            // 
            packToolStripMenuItem.Name = "packToolStripMenuItem";
            packToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            packToolStripMenuItem.Text = "Pack to .bml";
            packToolStripMenuItem.Click += packToolStripMenuItem_Click;
            // 
            // packTogslToolStripMenuItem
            // 
            packTogslToolStripMenuItem.Name = "packTogslToolStripMenuItem";
            packTogslToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            packTogslToolStripMenuItem.Text = "Pack to .gsl";
            packTogslToolStripMenuItem.Click += packTogslToolStripMenuItem_Click;
            // 
            // quitToolStripMenuItem
            // 
            quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            quitToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            quitToolStripMenuItem.Text = "Quit";
            quitToolStripMenuItem.Click += quitToolStripMenuItem_Click;
            // 
            // prsToolStripMenuItem
            // 
            prsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { compressPrsToolStripMenuItem, decompressPrsToolStripMenuItem });
            prsToolStripMenuItem.Name = "prsToolStripMenuItem";
            prsToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            prsToolStripMenuItem.Text = "Prs";
            // 
            // compressPrsToolStripMenuItem
            // 
            compressPrsToolStripMenuItem.Name = "compressPrsToolStripMenuItem";
            compressPrsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            compressPrsToolStripMenuItem.Text = "Compress Prs";
            compressPrsToolStripMenuItem.Click += compressPrsToolStripMenuItem_Click;
            // 
            // decompressPrsToolStripMenuItem
            // 
            decompressPrsToolStripMenuItem.Name = "decompressPrsToolStripMenuItem";
            decompressPrsToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            decompressPrsToolStripMenuItem.Text = "Decompress Prs";
            decompressPrsToolStripMenuItem.Click += decompressPrsToolStripMenuItem_Click;
            // 
            // bigEndianCheck
            // 
            bigEndianCheck.AutoSize = true;
            bigEndianCheck.Location = new System.Drawing.Point(14, 76);
            bigEndianCheck.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            bigEndianCheck.Name = "bigEndianCheck";
            bigEndianCheck.Size = new System.Drawing.Size(192, 19);
            bigEndianCheck.TabIndex = 1;
            bigEndianCheck.Text = "Pack to Big Endian (Gamecube)";
            bigEndianCheck.UseVisualStyleBackColor = true;
            bigEndianCheck.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(10, 51);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(96, 15);
            label1.TabIndex = 3;
            label1.Text = "Packing Options";
            // 
            // recursiveUnpackCB
            // 
            recursiveUnpackCB.AutoSize = true;
            recursiveUnpackCB.Location = new System.Drawing.Point(14, 28);
            recursiveUnpackCB.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            recursiveUnpackCB.Name = "recursiveUnpackCB";
            recursiveUnpackCB.Size = new System.Drawing.Size(218, 19);
            recursiveUnpackCB.TabIndex = 4;
            recursiveUnpackCB.Text = "Extract BML files within selected GSL";
            recursiveUnpackCB.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(249, 113);
            Controls.Add(recursiveUnpackCB);
            Controls.Add(label1);
            Controls.Add(bigEndianCheck);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = ".bml, .gsl Handler";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem packTogslToolStripMenuItem;
        private System.Windows.Forms.CheckBox recursiveUnpackCB;
    }
}

