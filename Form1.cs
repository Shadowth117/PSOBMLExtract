using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BmlExtract
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog;
        private bool bigEndian = false;
        private bool blueBurstPadding = false;
        public Form1()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog()
            {
                Filter =
                "PSO bml archive files (*.bml)|*.bml",
                Title = "Open bml file(s)"
            };
            openFileDialog.Multiselect = true;
            packToolStripMenuItem.Enabled = false;
            packToolStripMenuItem.Visible = false;
            bigEndianCheck.Enabled = false;
            bigEndianCheck.Visible = false;
            blueBurstPaddingCheck.Enabled = false;
            blueBurstPaddingCheck.Visible = false;
            label1.Visible = false;
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (String file in openFileDialog.FileNames)
                {
                    try
                    {
                        BMLUtil.ExtractBML(file);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: Could not read file {Path.GetFileName(file)} from disk. Original error: " + ex.Message);
                    }
                }
                
            }
        }

        private void packToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Why don't more people use this instead of the ugly, horrible other one?
            CommonOpenFileDialog goodOpenFileDialog = new CommonOpenFileDialog();
            goodOpenFileDialog.IsFolderPicker = true;
            if (goodOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                BMLUtil.PackBML(goodOpenFileDialog.FileName, bigEndian, blueBurstPadding);
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bigEndian = bigEndianCheck.Checked;
        }

        private void blueBurstPadding_CheckedChanged(object sender, EventArgs e)
        {
            blueBurstPadding = blueBurstPaddingCheck.Checked;
        }
    }
}
