using BmlExtract;
using csharp_prs;
using Reloaded.Memory.Streams;
using SonicRetro.SAModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using static BmlExtract.BMLUtil;

namespace PSOBMLHandler
{
    public unsafe class GSLUtil
    {
        public struct GSLFileEntry
        {
            public fixed byte filename[0x20];
            public int fileOffset;
            public int fileSize;
            public long padding;
        }

        public static void ExtractGSL(string fileName, bool recursive)
        {
            List<GSLFileEntry> fileTable = new List<GSLFileEntry>();
            List<string> debugText = new List<string>();

            if (File.Exists(fileName))
            {
                List<uint> fileStarts = new List<uint>();
                List<uint> pvmFileStarts = new List<uint>();
                List<uint> fileEnds = new List<uint>();
                List<uint> pvmFileEnds = new List<uint>();
                using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var streamReader = new BufferedStreamReader(fileStream, 8192))
                {
                    //GSL has no true file header
                    var testHeader = streamReader.Peek<GSLFileEntry>();
                    bool bigEndian = false;

                    //Handle big endian format for GC titles
                    if (testHeader.fileOffset > ToBigEndian((int)testHeader.fileOffset))
                    {
                        bigEndian = true;

                        testHeader.fileOffset = ToBigEndian(testHeader.fileOffset);
                        testHeader.fileSize = ToBigEndian(testHeader.fileSize);
                        testHeader.padding = ToBigEndian((int)testHeader.padding);
                    }

                    int firstFileOffset = testHeader.fileOffset * 0x800;
                    int strTest = streamReader.Peek<byte>();

                    for (int i = 0; streamReader.Position() < firstFileOffset && strTest != 0; i++)
                    {
                        GSLFileEntry entry = streamReader.Read<GSLFileEntry>();
                        string finalName;
                        GetGSLEntryFilename(entry, out finalName);
                        Console.WriteLine(finalName);
                        if (bigEndian)
                        {
                            entry.fileOffset = ToBigEndian(entry.fileOffset);
                            entry.fileSize = ToBigEndian(entry.fileSize);
                            entry.padding = ToBigEndian((int)entry.padding);
                        }
                        fileTable.Add(entry);

                        strTest = streamReader.Peek<byte>();
                    }
                    Console.WriteLine("Past header");

                    DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName)));

                    for (int i = 0; i < fileTable.Count; i++)
                    {
                        GSLFileEntry entry = fileTable[i];
                        var offset = entry.fileOffset * 0x800; 
                        streamReader.Seek(offset, SeekOrigin.Begin);

                        string finalName, outFileName;
                        GetGSLEntryFilename(entry, out finalName);
                        outFileName = dir.FullName + @"\" + finalName;

                        debugText.Add(finalName + " offset start, end: \n" + offset.ToString("X"));
                        fileStarts.Add((uint)offset);

                        byte[] file = streamReader.ReadBytes(offset, entry.fileSize);
                        offset += entry.fileSize;
                        fileEnds.Add((uint)offset);

                        debugText.Add(offset.ToString("X"));

                        File.WriteAllBytes(outFileName, file);

                        if(recursive)
                        {
                            ExtractBML(outFileName);
                        }
                    }

                }

#if DEBUG
                using (StreamWriter file = new StreamWriter(fileName + "_debug.txt"))
                {
                    file.WriteLine("GSL File Count: " + fileTable.Count);
                    for( int i = 0; i < fileTable.Count; i++)
                    {
                        GSLFileEntry bmlFile = fileTable[i];
                        GetGSLEntryFilename(bmlFile, out string finalName);
                        file.WriteLine("**" + finalName + "**");
                        file.WriteLine("Offset: " + fileStarts[i].ToString("X"));
                        file.WriteLine("End: " + fileEnds[i].ToString("X"));
                        file.WriteLine("Unknown: " + bmlFile.padding.ToString("X"));
                        file.WriteLine();
                    }
                    file.WriteLine("Actual Offsets");
                    foreach(string s in debugText)
                    {
                        file.WriteLine(s);
                    }
                }
#endif
            }
        }


        public static void PackGSL(string filePath, bool bigEndian)
        {
            List<string> fileList = new List<string>();
            Dictionary<string, List<string>> fileDict = new Dictionary<string, List<string>>();
            List<GSLFileEntry> header = new List<GSLFileEntry>();
            List<byte> headerEntries = new List<byte>();
            List<byte> fileData = new List<byte>();

            //Sort files close to the same way as sega for gsl
            foreach(var file in Directory.GetFiles(filePath))
            {
                var ext = Path.GetExtension(file);

                //This one in blue bursts's gsl should be a bml, but gets cut off
                if(ext == "")
                {
                    ext = ".bml";
                }
                if(fileDict.ContainsKey(ext))
                {
                    fileDict[ext].Add(file);
                } else
                {
                    fileDict.Add(ext, new List<string>() { file });
                }
            }

            List<string> fileTypes = fileDict.Keys.ToList();
            fileTypes.Sort();
            foreach(var type in fileTypes)
            {
                var list = fileDict[type];
                list.Sort(StringComparer.OrdinalIgnoreCase);
                foreach (var entry in list)
                {
                    fileList.Add(entry);
                }
            }

            int fileEntriesLength = fileList.Count * 0x30;
            int firstFileOffset = (0x800 - (fileEntriesLength % 0x800)) + fileEntriesLength;
            int currentFileOffset = firstFileOffset;

            for (int j = 0; j < fileList.Count; j++)
            {
                string s = fileList[j];
                GSLFileEntry entry = new GSLFileEntry();

                byte[] file = File.ReadAllBytes(s);

                entry.fileSize = file.Length;
                entry.fileOffset = currentFileOffset / 0x800;
                if (bigEndian)
                {
                    entry.fileSize = ToBigEndian(entry.fileSize);
                    entry.fileOffset = ToBigEndian(entry.fileOffset);
                }

                //There's a bml in blue burst's data.gsl that goes over the text limit and since the final original character is a period and you cannot have empty extensions in Windows, we add the period
                var ext = Path.GetExtension(s);
                string finalName = s;
                if(ext == "")
                {
                    finalName += ".";
                }
                byte[] filename = System.Text.Encoding.UTF8.GetBytes(Path.GetFileName(finalName));

                for (int i = 0; i < filename.Length; i++)
                {
                    entry.filename[i] = filename[i];
                    if (i == 0x20)
                    {
                        break;
                    }
                }
                headerEntries.AddRange(Reloaded.Memory.Struct.GetBytes(entry));

                int padding = 0;
                if(j + 1 != fileList.Count)
                {
                    padding = 0x800 - (file.Length % 0x800);
                    if(padding == 0x800)
                    {
                        padding = 0;
                    }
                }
                currentFileOffset += file.Length + padding;
                fileData.AddRange(file);
                fileData.AddRange(new byte[padding]);
            }
            headerEntries.AddRange(new byte[(0x800 - (fileEntriesLength % 0x800))]);
            fileData.InsertRange(0, headerEntries);

            File.WriteAllBytes(filePath + "_new.gsl", fileData.ToArray());

        }

        public static void GetGSLEntryFilename(GSLFileEntry entry, out string finalName)
        {
            //Lazily determine string end
            int end = 0x20;
            for (int j = 0; j < 0x20; j++)
            {
                if (entry.filename[j] == 0)
                {
                    end = j;
                    break;
                }
            }


            byte[] entryFilename = new byte[end];
            Marshal.Copy(new IntPtr(entry.filename), entryFilename, 0, end);
            finalName = System.Text.Encoding.UTF8.GetString(entryFilename);

        }
    }
}
