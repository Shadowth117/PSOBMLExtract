using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BmlExtract
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool bigEndian = false;
            bool blueBurst = true;
            if(args.Length > 0)
            {
                if (args.Contains(":BE"))
                {
                    bigEndian = true;
                }
                if (args.Contains(":BB"))
                {
                    blueBurst = true;
                }
                foreach (string s in args)
                {
                    if (Path.GetExtension(s).Equals(""))
                    {
                        BMLUtil.PackBML(s, bigEndian, blueBurst);
                    }
                    else
                    {
                        BMLUtil.ExtractBML(s);
                    }
                }
            } else
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
