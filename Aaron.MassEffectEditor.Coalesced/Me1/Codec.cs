using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Aaron.MassEffectEditor.Coalesced.Records;
using Aaron.MassEffectEditor.Core;

namespace Aaron.MassEffectEditor.Coalesced.Me1
{
    class Codec : ICodec
    {

        public const int DEFAULT_STRING = 0;
        public Games Game => Games.Me1;

        public string Name { get; }

        public Codec(string name)
        {
            Name = name;
        }

        public Codec()
            :this("Mass Effect 1 Codec") { }

        public byte[] Encode(Container value)
        {
            MemoryStream memoryStream = new();
            BinaryWriter output = new(memoryStream);

            output.Write(value.Files.Count);
            foreach (FileRecord fileRecord in value.Files)
            {
                WriteString(fileRecord.Name, output);

                output.Write(fileRecord.Count);
                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    WriteString(sectionRecord.Name, output);

                    output.Write(fileRecord.Count);
                    foreach (EntryRecord entryRecord in sectionRecord)
                    {
                        WriteString(entryRecord.Name, output);
                        WriteString(entryRecord.First(), output);
                    }
                }


            }

            return memoryStream.ToArray();
        }

        public Container Decode(byte[] value)
        {
            BinaryReader input = new(new MemoryStream(value));
            Container container = new();

            int fileCount = input.ReadInt32();
            for (int fileIndex = 0; fileIndex < fileCount; fileIndex++)
            {
                FileRecord fileRecord = new() {Name = ReadString(input)};

                int sectionCount = input.ReadInt32();
                for (int sectionIndex = 0; sectionIndex < sectionCount; sectionIndex++)
                {
                    SectionRecord sectionRecord = new() {Name = ReadString(input)};
                    
                    int entryCount = input.ReadInt32();
                    for (int entryIndex = 0; entryIndex < entryCount; entryIndex++)
                    {
                        EntryRecord entryRecord = new(){ Name = ReadString(input) };
                        entryRecord.Add(ReadString(input));

                        sectionRecord.Add(entryRecord);
                    }

                    fileRecord.Add(sectionRecord);
                }

                container.Files.Add(fileRecord);
            }

            return container;
        }

        private static string ReadString(BinaryReader input)
        {
            int stringLength = (input.ReadInt32() * -2); //string lengths are the negative number of characters?

            if (stringLength == 0)
            {
                return string.Empty;
            }

            byte[] stringBuffer = input.ReadBytes(stringLength);

            return Encoding.Unicode.GetString(stringBuffer, 0, stringBuffer.Length - 2);
        }

        private static void WriteString(string value, BinaryWriter output)
        {
            if (string.IsNullOrEmpty(value))
            {
                output.Write(DEFAULT_STRING);
                return;
            }

            int stringLength = (value.Length + 1) * -1;
            byte[] stringBuffer = Encoding.Unicode.GetBytes(value + '\0');

            output.Write(stringLength);
            output.Write(stringBuffer);
        }

        public void Dump()
        {
            
        }
    }
}
