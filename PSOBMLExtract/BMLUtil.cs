using csharp_prs;
using Reloaded.Memory.Streams;
using SonicRetro.SAModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BmlExtract
{
    unsafe class BMLUtil
    {
        //Struct info from: http://sharnoth.com/psodevwiki/format/bml
        public struct BMLHeader
        {
            public int unkInt0;
            public int numFiles;
            public int magicNumber; //0x150 
            public fixed int padding[0xD];
        }

        public struct BMLFileEntry
        {
            public fixed byte filename[0x20];
            public int compressedSize;
            public int unkInt0;
            public int decompressedSize;
            public int pvmCompressedSize;
            public int pvmDecompressedSize;
            public fixed int padding[0x3];
        }

        public class BMLData
        {
            public byte[] file = null;
            public byte[] tex = null;
        }

        public static void ExtractBML(string fileName)
        {
            BMLHeader bmlHeader;
            List<BMLFileEntry> fileTable = new List<BMLFileEntry>();
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
                    bmlHeader = streamReader.Read<BMLHeader>();
                    bool bigEndian = false;

                    //Handle big endian format for GC titles
                    if (bmlHeader.numFiles > 0xFFFF)
                    {
                        bmlHeader.numFiles = ToBigEndian(bmlHeader.numFiles);
                        bigEndian = true;
                    }

                    for (int i = 0; i < bmlHeader.numFiles; i++)
                    {
                        BMLFileEntry entry = streamReader.Read<BMLFileEntry>();
                        string finalName;
                        GetBMLFileEntryFilename(entry, out finalName);
                        Console.WriteLine(finalName);
                        if (bigEndian)
                        {
                            entry.compressedSize = ToBigEndian(entry.compressedSize);
                            entry.unkInt0 = ToBigEndian(entry.unkInt0);
                            entry.decompressedSize = ToBigEndian(entry.decompressedSize);
                            entry.pvmCompressedSize = ToBigEndian(entry.pvmCompressedSize);
                            entry.pvmDecompressedSize = ToBigEndian(entry.pvmDecompressedSize);
                        }
                        fileTable.Add(entry);
                        
                    }
                    Console.WriteLine("Past header");

                    DirectoryInfo dir = Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName)));

                    int offset = 0;

                    //Determines where file offsets should start. See: https://github.com/bogglez/Ninja-Lib/blob/master/src/bml.js#L45
                    if ((streamReader.Position() & 0x7FF) > 0)
                    {
                        offset = ((int)((streamReader.Position() + 0x800) & 0xFFFFF800));
                    } else
                    {
                        offset = (int)streamReader.Position();
                    }

                    for (int i = 0; i < fileTable.Count; i++)
                    {
                        BMLFileEntry entry = fileTable[i];

                        string finalName, outFileName;
                        GetBMLFileEntryFilename(entry, out finalName);
                        outFileName = dir.FullName + @"\" + finalName;

                        debugText.Add(finalName + " offset start, end: \n" + offset.ToString("X"));
                        fileStarts.Add((uint)offset);

                        byte[] prsFile = streamReader.ReadBytes(offset, entry.compressedSize);
                        offset += entry.compressedSize;
                        fileEnds.Add((uint)offset);

                        debugText.Add(offset.ToString("X"));
                        offset = SeekPadding(fileTable, streamReader, offset, i);

                        

                        File.WriteAllBytes(outFileName, Prs.Decompress(prsFile));

                        //Get textures, if they're there. Should be pvm, gvm, or xvm assumedly
                        if (entry.pvmCompressedSize > 0)
                        {
                            pvmFileStarts.Add((uint)offset);
                            streamReader.Seek(offset, SeekOrigin.Begin);
                            string fileType = System.Text.Encoding.UTF8.GetString(BitConverter.GetBytes(streamReader.Peek<int>())).Substring(1).ToLower();
                            debugText.Add(finalName + "." + fileType + " offset start, end: \n" + offset.ToString("X"));

                            byte[] prsTexFile = streamReader.ReadBytes(offset, entry.pvmCompressedSize);
                            File.WriteAllBytes(outFileName + "." + fileType, Prs.Decompress(prsTexFile));
                            offset += entry.pvmCompressedSize;
                            pvmFileEnds.Add((uint)offset);

                            debugText.Add(offset.ToString("X"));
                            offset = SeekPadding(fileTable, streamReader, offset, i);
                        } else
                        {
                            pvmFileStarts.Add(0);
                            pvmFileEnds.Add(0);
                        }
                    }

                }

#if DEBUG
                using (StreamWriter file = new StreamWriter(fileName + "_debug.txt"))
                {
                    file.WriteLine("BML File Count: " + bmlHeader.numFiles);
                    for( int i = 0; i < fileTable.Count; i++)
                    {
                        BMLFileEntry bmlFile = fileTable[i];
                        GetBMLFileEntryFilename(bmlFile, out string finalName);
                        file.WriteLine("**" + finalName + "**");
                        file.WriteLine("Offset: " + fileStarts[i].ToString("X"));
                        file.WriteLine("End: " + fileEnds[i].ToString("X"));
                        file.WriteLine("PVM Offset: " + pvmFileStarts[i].ToString("X"));
                        file.WriteLine("PVM End: " + pvmFileEnds[i].ToString("X"));
                        file.WriteLine("Compressed Size: " + bmlFile.compressedSize.ToString("X"));
                        file.WriteLine("Unknown: " + bmlFile.unkInt0.ToString("X"));
                        file.WriteLine("Decompressed Size: " + bmlFile.decompressedSize.ToString("X"));
                        file.WriteLine("PVM Compressed Size: " + bmlFile.pvmCompressedSize.ToString("X"));
                        file.WriteLine("PVM Decompressed Size: " + bmlFile.pvmDecompressedSize.ToString("X"));
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

        public static void PackBML(string filePath, bool bigEndian)
        {
            List<string> rawfileList = new List<string>();
            List<string> pairCheck = new List<string>();
            List<BMLFileEntry> header = new List<BMLFileEntry>();
            List<byte> data = new List<byte>();
            BMLHeader head = new BMLHeader();

            rawfileList.AddRange(Directory.GetFiles(filePath));
            SortedDictionary<string, string> pairedFiles = new SortedDictionary<string, string>();
            SortedSet<string> nonPairedFiles = new SortedSet<string>();
            
            foreach(string s in rawfileList)
            {
                string modelFile = Path.GetDirectoryName(s) + @"\" + Path.GetFileNameWithoutExtension(s);
                if (rawfileList.Contains(modelFile))
                {
                    pairedFiles.Add(modelFile, s);
                    pairCheck.Add(modelFile);
                    pairCheck.Add(s);
                }
            }
            foreach(string s in rawfileList)
            {
                if(!pairCheck.Contains(s))
                {
                    nonPairedFiles.Add(s);
                }
            }

            head.numFiles = pairedFiles.Keys.Count + nonPairedFiles.Count;
            head.magicNumber = 0x150;
            if (bigEndian)
            {
                head.numFiles = ToBigEndian(head.numFiles);
            }

            int unknownThing = 0;
            List<BMLData> pairedBMLData = new List<BMLData>();
            List<byte[]> singleBMLData = new List<byte[]>();
            foreach(string s in pairedFiles.Keys)
            {
                BMLFileEntry entry = new BMLFileEntry();

                byte[] model = File.ReadAllBytes(s);
                byte[] tex = File.ReadAllBytes(pairedFiles[s]);
                byte[] cmpModel = Prs.Compress(model, 0x1FFF);
                byte[] cmpTex = Prs.Compress(tex, 0x1FFF);

                entry.unkInt0 = unknownThing;
                entry.decompressedSize = model.Length;
                entry.pvmDecompressedSize = tex.Length;
                entry.compressedSize = cmpModel.Length;
                entry.pvmCompressedSize = cmpTex.Length;
                if (bigEndian)
                {
                    entry.decompressedSize = ToBigEndian(entry.decompressedSize);
                    entry.compressedSize = ToBigEndian(entry.compressedSize);
                    entry.pvmDecompressedSize = ToBigEndian(entry.pvmDecompressedSize);
                    entry.pvmCompressedSize = ToBigEndian(entry.pvmCompressedSize);
                }

                byte[] filename = System.Text.Encoding.UTF8.GetBytes(Path.GetFileName(s));

                for(int i = 0; i < filename.Length; i++)
                {
                    entry.filename[i] = filename[i];
                    if(i == 0x20)
                    {
                        break;
                    }
                }
                header.Add(entry);

                pairedBMLData.Add(new BMLData() { file = cmpModel, tex = cmpTex });
            }

            foreach (string s in nonPairedFiles)
            {
                BMLFileEntry entry = new BMLFileEntry();

                byte[] model = File.ReadAllBytes(s);
                byte[] cmpFile = Prs.Compress(model, 0x1FFF);

                entry.unkInt0 = unknownThing;
                entry.decompressedSize = model.Length;
                entry.pvmDecompressedSize = 0;
                entry.compressedSize = cmpFile.Length;
                entry.pvmCompressedSize = 0;
                if(bigEndian)
                {
                    entry.decompressedSize = ToBigEndian(entry.decompressedSize);
                    entry.compressedSize = ToBigEndian(entry.compressedSize);
                }

                byte[] filename = System.Text.Encoding.UTF8.GetBytes(Path.GetFileName(s));

                for (int i = 0; i < filename.Length; i++)
                {
                    entry.filename[i] = filename[i];
                    if (i == 0x20)
                    {
                        break;
                    }
                }
                header.Add(entry);

                singleBMLData.Add(cmpFile);
            }

            List<byte> entrySet = new List<byte>();

            entrySet.AddRange(Reloaded.Memory.Struct.GetBytes(head));
            foreach(BMLFileEntry entry in header)
            {
                entrySet.AddRange(Reloaded.Memory.Struct.GetBytes(entry));
            }
            int entryEnding = sizeof(BMLHeader) + sizeof(BMLFileEntry) * header.Count;
            int fileStartOffset;

            if ((entryEnding & 0x7FF) > 0)
            {
                fileStartOffset = ((int)((entryEnding + 0x800) & 0xFFFFF800));
            }
            else
            {
                fileStartOffset = (int)entryEnding;
            }

            while(entrySet.Count < fileStartOffset)
            {
                entrySet.Add(0);
            }

            data.AddRange(entrySet);

            foreach(var pair in pairedBMLData)
            {
                data.AddRange(pair.file);
                data.Align(0x20);

                if (pair.tex != null)
                {
                    data.AddRange(pair.tex);
                    data.Align(0x20);
                }
            }

            foreach (var file in singleBMLData)
            {
                data.AddRange(file);
                data.Align(0x20);
            }

            File.WriteAllBytes(filePath + "_new.bml", data.ToArray());
                   
        }

        public static void GetBMLFileEntryFilename(BMLFileEntry entry, out string finalName)
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

        public static int ToBigEndian(int smallInt)
        {
            byte[] numFilesBytes = BitConverter.GetBytes(smallInt);
            Array.Reverse(numFilesBytes);

            smallInt = BitConverter.ToInt32(numFilesBytes, 0);
            return smallInt;
        }

        private static int SeekPadding(List<BMLFileEntry> fileTable, BufferedStreamReader streamReader, int offset, int i)
        {
            while (offset % 0x10 > 0)
            {
                offset += 1;
            }
            streamReader.Seek(offset, SeekOrigin.Begin);

            while(streamReader.Peek<int>() == 0 && streamReader.CanRead(4))
            {
                offset += 0x10;
                streamReader.Seek(offset, SeekOrigin.Begin);
            }

            return offset;
        }

        public static byte[] PRSDecompressFile(byte[] file)
        {
            return Prs.Decompress(file);
        }

        public static byte[] PRSCompressFile(byte[] file)
        {
            return Prs.Compress(file, 0x1FFF);
        }
    }
}
