using Microsoft.WindowsAPICodePack.Dialogs;
using PSOBMLHandler;
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
        private OpenFileDialog openFileDialogDecompressPRS;
        private OpenFileDialog openFileDialogCompressPRS;
        private bool bigEndian = false;
        public Form1()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog()
            {
                Filter =
                "PSO bml, gsl archive files (*.bml, *.gsl)|*.bml;*.gsl|PSO bml archive files (*.bml)|*.bml|PSO gsl archive files (*.gsl)|*.gsl",
                Title = "Open bml file(s)"
            };
            openFileDialogDecompressPRS = new OpenFileDialog()
            {
                Filter =
                "Prs compressed files (*.prs)|*.prs|All files|*.*",
                Title = "Select prs compressed file(s)"
            };
            openFileDialogCompressPRS = new OpenFileDialog()
            {
                Title = "Select uncompressed file(s) to compress to prs"
            };
            openFileDialog.Multiselect = true;
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
                        switch (Path.GetExtension(file).ToLower())
                        {
                            case ".gsl":
                                GSLUtil.ExtractGSL(file, recursiveUnpackCB.Checked);
                                break;
                            case ".bml":
                            default:
                                BMLUtil.ExtractBML(file);
                                break;
                        }
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
                BMLUtil.PackBML(goodOpenFileDialog.FileName, bigEndian);
            }

        }
        private void packTogslToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Seriously, the normal one is just bad
            CommonOpenFileDialog goodOpenFileDialog = new CommonOpenFileDialog();
            goodOpenFileDialog.IsFolderPicker = true;
            if (goodOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                GSLUtil.PackGSL(goodOpenFileDialog.FileName, bigEndian);
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            bigEndian = bigEndianCheck.Checked;
        }

        private void compressPrsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialogCompressPRS.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(openFileDialogCompressPRS.FileName + ".prs", BMLUtil.PRSCompressFile(File.ReadAllBytes(openFileDialogCompressPRS.FileName)));
            }
        }

        private void decompressPrsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialogDecompressPRS.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(openFileDialogDecompressPRS.FileName + ".bin", BMLUtil.PRSDecompressFile(File.ReadAllBytes(openFileDialogDecompressPRS.FileName)));
            }
        }
    }
}
