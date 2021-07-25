using Aaron.MassEffectEditor.Core;
using System.Collections;
using System.IO;

namespace Aaron.MassEffectEditor.Coalesced.Me3
{
    internal class Codec : ICodec
    {
        public string Name { get; set; }

        public HeaderBlock Header { get; } = new();
        public StringTableBlock StringTable { get; } = new();
        public HuffmanTreeBlock HuffmanTree { get; } = new();
        public DataBlock Data { get; } = new();
        public BitArray CompressedData { get; set; }

        public Container Container { get; set; }

        public Codec(string name)
        {
            Name = name;
        }

        public Codec()
        {
            Name = "Codec";
        }

        public Games Game { get; } = Games.Me3;

        public Container Decode(byte[] value)
        {
            BinaryReader input = new(new MemoryStream(value));

            Header.Read(input, this);
            StringTable.Read(input.ReadBytes((int) Header.StringTableLength), this);
            HuffmanTree.Read(input.ReadBytes((int) Header.HuffmanLength), this);

            byte[] indexData = input.ReadBytes((int) Header.IndexLength);

            int compressedDataLength =
                input.ReadInt32(); //TODO: This should really be in the DataBlock, its just easier to do it here because if the interface
            CompressedData = new BitArray(input.ReadBytes((int) Header.DataLength));

            Data.Read(indexData, this);

            Container = Data.Container;

            return Data.Container;
        }

        public byte[] Encode(Container value)
        {
            Container = value;

            MemoryStream outputStream = new();
            BinaryWriter output = new(outputStream);
            outputStream.Position = HeaderBlock.HEADER_LENGTH;

            StringTable.Write(output, this);
            HuffmanTree.Write(output, this);
            Data.Write(output, this);

            outputStream.Position = 0;
            Header.Write(output, this);

            outputStream.Flush();
            return outputStream.ToArray();
        }

        public void Dump()
        {
            Header.Dump(Name);
            StringTable.Dump(Name);
            HuffmanTree.Dump(Name);
            Data.Dump(Name);
        }
    }
}