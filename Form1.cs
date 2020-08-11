using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BmlExtract
{
    public partial class Form1 : Form
    {
        private OpenFileDialog openFileDialog;

        public Form1()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog()
            {
                Filter =
                "PSO bml archive files (*.bml)|*.bml",
                Title = "Open bml file"
            };
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                BMLUtil.ExtractBML(openFileDialog.FileName);
            }
        }
    }
}
