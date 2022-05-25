using BmlExtract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSOBMLExtract
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            bool bigEndian = false;
            int prs = 0;
            foreach(var str in args)
            {
                switch(str)
                {
                    case "-be":
                        bigEndian = true;
                        break;
                    case "-le":
                        bigEndian = false;
                        break;
                    case "-prsDec":
                        prs = -1;
                        break;
                    case "-prsCmp":
                        prs = 1;
                        break;
                    case "-noPrs":
                        prs = 0;
                        break;
                    default:
                        switch(prs)
                        {
                            case 1:
                                File.WriteAllBytes(str + ".prs", BMLUtil.PRSCompressFile(File.ReadAllBytes(str)));
                                break;
                            case -1:
                                if (File.Exists(str))
                                {
                                    File.WriteAllBytes(str + ".bin", BMLUtil.PRSDecompressFile(File.ReadAllBytes(str)));
                                }
                                break;
                            default:
                                if (Directory.Exists(str))
                                {
                                    BMLUtil.PackBML(str, bigEndian);
                                }
                                else if (File.Exists(str))
                                {
                                    BMLUtil.ExtractBML(str);
                                }
                                break;
                        }
                        break;
                }
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
