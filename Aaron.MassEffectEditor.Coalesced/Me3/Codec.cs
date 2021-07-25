using Aaron.MassEffectEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Me3
{
    class Codec : ICodec
    {
        public Games Game { get; private set; } = Games.Me3;

        public string Name { get; set; }

        public Codec(string name)
        {
            Name = name;
        }

        public Codec()
        {
            Name = "Codec";
        }

        public HeaderBlock Header { get; private set; } = new HeaderBlock();
        public StringTableBlock StringTable { get; private set; } = new StringTableBlock();
        public HuffmanTreeBlock HuffmanTree { get; private set; } = new HuffmanTreeBlock();
        public DataBlock Data { get; private set; } = new DataBlock();
        public BitArray CompressedData { get; set; }

        public Container Container { get; set; }

        public Container Decode(byte[] value)
        {
            BinaryReader input = new BinaryReader(new MemoryStream(value));

            Header.Read(input, this);
            StringTable.Read(input.ReadBytes((int)Header.StringTableLength), this);
            HuffmanTree.Read(input.ReadBytes((int)Header.HuffmanLength), this);

            byte[] indexData = input.ReadBytes((int)Header.IndexLength);

            int compressedDataLength = input.ReadInt32(); //TODO: This should really be in the DataBlock, its just eaiser to do it here because if the interface
            CompressedData = new BitArray(input.ReadBytes((int)Header.DataLength));

            Data.Read(indexData, this);

            Container = Data.Container;

            return Data.Container;
        }

        public byte[] Encode(Container value)
        {
            Container = value;

            MemoryStream outputStream = new MemoryStream();
            BinaryWriter output = new BinaryWriter(outputStream);
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
