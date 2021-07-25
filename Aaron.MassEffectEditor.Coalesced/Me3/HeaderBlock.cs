using Aaron.MassEffectEditor.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Me3
{
    class HeaderBlock : IBlock<Codec>
    {
        public const uint MAGIC_WORD = 0x666D726D;

        public const int HEADER_LENGTH = 8 * 4;

        public uint MagicWord { get; set; }
        public uint Version { get; set; }

        public int MaxKeyLength { get; set; }
        public int MaxValueLength { get; set; }

        public uint StringTableLength { get; set; }

        public uint HuffmanLength { get; set; }

        public uint IndexLength { get; set; }

        public uint DataLength { get; set; }


        public void Validate(Codec codec)
        {
            if (MagicWord != MAGIC_WORD)
            {
                throw new FormatException($"The file does not begin with the correct magic word 0x{MAGIC_WORD:X}");
            }

            if (Version != 1)
            {
                throw new FormatException("We can only parse version 1 of this file");
            }
        }

        public void Read(byte[] data, Codec codec)
        {
            BinaryReader input = new(new MemoryStream(data));

            Read(input, codec);
        }



        public void Read(BinaryReader input, Codec codec)
        {
            MagicWord = input.ReadUInt32();
            Version = input.ReadUInt32();
            Validate(codec);
            MaxKeyLength = input.ReadInt32();
            MaxValueLength = input.ReadInt32();
            StringTableLength = input.ReadUInt32();
            HuffmanLength = input.ReadUInt32();
            IndexLength = input.ReadUInt32();
            DataLength = input.ReadUInt32();
        }



        public void Write(BinaryWriter output, Codec codec)
        {
            MagicWord = MAGIC_WORD;
            Version = 1;

            output.Write(MagicWord);
            output.Write(Version);
            output.Write(MaxKeyLength);
            output.Write(MaxValueLength);
            output.Write(StringTableLength);
            output.Write(HuffmanLength);
            output.Write(IndexLength);
            output.Write(DataLength);
        }

        public bool Match(HeaderBlock other)
        {
            return (MagicWord == other.MagicWord)
                && (Version == other.Version)
                && (MaxKeyLength == other.MaxKeyLength)
                && (MaxValueLength == other.MaxValueLength)
                && (StringTableLength == other.StringTableLength)
                && (HuffmanLength == other.HuffmanLength)
                && (IndexLength == other.IndexLength)
                && (DataLength == other.DataLength);
        }

        public string Dump()
        {
            StringBuilder output = new();

            output.AppendLine($"          MagicWord = {MagicWord,16}");
            output.AppendLine($"            Version = {Version,16}");
            output.AppendLine($"       MaxKeyLength = {MaxKeyLength,16}");
            output.AppendLine($"     MaxValueLength = {MaxValueLength,16}");
            output.AppendLine($"  StringTableLength = {StringTableLength,16}");
            output.AppendLine($"      HuffmanLength = {HuffmanLength,16}");
            output.AppendLine($"        IndexLength = {IndexLength,16}");
            output.AppendLine($"         DataLength = {DataLength,16}");

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = $"{rootName}.header.txt";
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }

    }
}
