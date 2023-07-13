using BmlExtract;
using PSOBMLHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            bool recursive = false;
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
                    case "-prsdec":
                        prs = -1;
                        break;
                    case "-prscmp":
                        prs = 1;
                        break;
                    case "-noprs":
                        prs = 0;
                        break;
                    case "-recursive":
                        recursive = true;
                        break;
                    default:
                        switch(prs)
                        {
                            case 1:
                                if (File.Exists(str))
                                {
                                    try
                                    {
                                        File.WriteAllBytes(str + ".prs", BMLUtil.PRSCompressFile(File.ReadAllBytes(str)));
                                    }
                                    catch
                                    {
                                        Trace.WriteLine($"Unable to PRS Compress {str}");
                                    }
                                }
                                break;
                            case -1:
                                if (File.Exists(str))
                                {
                                    try
                                    {
                                        File.WriteAllBytes(str + ".bin", BMLUtil.PRSDecompressFile(File.ReadAllBytes(str)));
                                    }
                                    catch
                                    {
                                        Trace.WriteLine($"Unable to PRS Decompress {str}");
                                    }
                                }
                                break;
                            default:
                                if (Directory.Exists(str))
                                {
                                    try
                                    {
                                        BMLUtil.PackBML(str, bigEndian);
                                    }
                                    catch
                                    {
                                        Trace.WriteLine($"Unable to pack BML from {str}");
                                    }
                                }
                                else if (File.Exists(str))
                                {
                                    try
                                    {
                                        switch (Path.GetExtension(str).ToLower())
                                        {
                                            case ".gsl":
                                                GSLUtil.ExtractGSL(str, recursive);
                                                break;
                                            case ".bml":
                                            default:
                                                BMLUtil.ExtractBML(str);
                                                break;
                                        }
                                    }
                                    catch
                                    {
                                        Trace.WriteLine($"Unable to extract BML {str}");
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
            if(args.Length > 0)
            {
                return;
            } else
            {
                Trace.WriteLine("PSO BML Handler by Shadowth117\n" +
                    "usage:\n" +
                    "Provide file(s) or directory(s) as arguments to pack or unpack.\nLittle endian bml handling is default. Commands to alter packing in entities following said commands are:\n" +
                    "-be : big endian bml packing (Only alters the bml itself, not the files within)\n" +
                    "-le : little endian bml packing [Default] (Only alters the bml itself, not the files within)\n" +
                    "-prsdec : Sets later files to attempt to be decompressed\n" +
                    "-prscmp : Sets later files to be prs decompressed\n" +
                    "-noprs : Sets back to BML mode" +
                    "-recursive : When extracting GSL archives, also extract BML archives found within the GSL archive");
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
