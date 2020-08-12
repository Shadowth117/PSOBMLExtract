using csharp_prs;
using Reloaded.Memory.Streams;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BmlExtract
{
    unsafe class BMLUtil
    {
        //Struct info from: http://sharnoth.com/psodevwiki/format/bml
        struct BMLHeader
        {
            public int unkInt0;
            public int numFiles;
            public int magicNumber; //0x150 
            public fixed int padding[0xD];
        }

        struct BMLFileEntry
        {
            public fixed byte filename[0x20];
            public int compressedSize;
            public int unkInt0;
            public int decompressedSize;
            public int pvmCompressedSize;
            public int pvmDecompressedSize;
            public fixed int padding[0x3];
        }

        public static void ExtractBML(string fileName)
        {
            BMLHeader bmlHeader;
            List<BMLFileEntry> fileTable = new List<BMLFileEntry>();

            if (File.Exists(fileName))
            {
                using (var fileStream = new FileStream(fileName, FileMode.Open))
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
                        if (bigEndian)
                        {
                            entry.compressedSize = ToBigEndian(entry.compressedSize);
                            entry.decompressedSize = ToBigEndian(entry.decompressedSize);
                            entry.pvmCompressedSize = ToBigEndian(entry.pvmCompressedSize);
                            entry.pvmDecompressedSize = ToBigEndian(entry.pvmDecompressedSize);
                        }
                        fileTable.Add(entry);
                        
                    }

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

                        byte[] prsFile = streamReader.ReadBytes(offset, entry.compressedSize);
                        offset += entry.compressedSize;

                        offset = AddPadding(fileTable, streamReader, offset, i);
                        
                        //Lazily determine string end
                        int end = 0;
                        for(int j = 0; j < 0x20; j++)
                        {
                            if(entry.filename[j] == 0)
                            {
                                end = j;
                                break;
                            }
                        }
                        byte[] entryFilename = new byte[end];
                        Marshal.Copy(new IntPtr(entry.filename), entryFilename, 0, end);
                        string finalName = System.Text.Encoding.UTF8.GetString(entryFilename);
                        string outFileName = dir.FullName + @"\" + finalName;
                        File.WriteAllBytes(outFileName, Prs.Decompress(prsFile));

                        //Get textures, if they're there. Should be pvm, gvm, or xvm assumedly
                        if (entry.pvmCompressedSize > 0)
                        {
                            streamReader.Seek(offset, SeekOrigin.Begin);
                            string fileType = System.Text.Encoding.UTF8.GetString(BitConverter.GetBytes(streamReader.Peek<int>())).Substring(1).ToLower();
                            byte[] prsTexFile = streamReader.ReadBytes(offset, entry.pvmCompressedSize);
                            File.WriteAllBytes(outFileName + "." + fileType, Prs.Decompress(prsTexFile));

                            offset += entry.pvmCompressedSize;

                            offset = AddPadding(fileTable, streamReader, offset, i);
                        }
                    }

                }
            }

        }

        private static int ToBigEndian(int smallInt)
        {
            byte[] numFilesBytes = BitConverter.GetBytes(smallInt);
            Array.Reverse(numFilesBytes);

            smallInt = BitConverter.ToInt32(numFilesBytes, 0);
            return smallInt;
        }

        private static int AddPadding(List<BMLFileEntry> fileTable, BufferedStreamReader streamReader, int offset, int i)
        {
            while (offset % 0x10 > 0)
            {
                offset += 1;
            }
            streamReader.Seek(offset, SeekOrigin.Begin);

            if (streamReader.Peek<int>() == 0)
            {
                offset += 0x10;
            }

            return offset;
        }
    }
}
