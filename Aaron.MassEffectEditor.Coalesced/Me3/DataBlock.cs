using Aaron.MassEffectEditor.Coalesced.Records;
using Aaron.MassEffectEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Huffman = Aaron.MassEffectEditor.Core.Compression.Huffman;

namespace Aaron.MassEffectEditor.Coalesced.Me3
{
    class DataBlock : IBlock<Codec>
    {
        public Container Container { get; set; } = new Container();

        public void Read(byte[] data, Codec codec)
        {            
            BinaryReader indexInput = new BinaryReader(new MemoryStream(data));

            DataStructures.IndexContainer indexContainer = new DataStructures.IndexContainer();
            indexContainer.Read(indexInput);
            indexContainer.Dump(codec.Name);

            Container = indexContainer.ToRecords(codec.StringTable, codec.HuffmanTree, codec.CompressedData, codec.Header.MaxValueLength);
        }
        public void Write(BinaryWriter output, Codec codec)
        {

            MemoryStream bufferStream = new MemoryStream();
            BinaryWriter buffer = new BinaryWriter(bufferStream);

            BitArray compressedData = new BitArray(codec.HuffmanTree.Encoder.TotalBits);

            var indexContainer = DataStructures.IndexContainer.FromRecords(Container, codec.StringTable, codec.HuffmanTree.Encoder, compressedData);
            int expectedLength = indexContainer.TotalSize();
            indexContainer.Write(buffer);

            byte[] indexData = bufferStream.ToArray();
            var data = new byte[(compressedData.Length - 1) / 8 + 1];
            compressedData.CopyTo(data, 0);

            codec.Header.IndexLength = (uint)indexData.Length;
            codec.Header.DataLength = (uint)data.Length;

            //TODO - Seek?
            output.Write(indexData);
            output.Write(compressedData.Length);
            output.Write(data);

        }

        public void Validate(Codec codec)
        {
            throw new NotImplementedException();
        }

        public string Dump()
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("Nothing here yet");

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = string.Format("{0}.data.txt", rootName);
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }


    }
}
