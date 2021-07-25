using Aaron.MassEffectEditor.Core;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Aaron.MassEffectEditor.Coalesced.Me3
{
    class DataBlock : IBlock<Codec>
    {
        public Container Container { get; set; } = new();

        public void Read(byte[] data, Codec codec)
        {
            BinaryReader indexInput = new(new MemoryStream(data));

            DataStructures.IndexContainer indexContainer = new();
            indexContainer.Read(indexInput);
            indexContainer.Dump(codec.Name);

            Container = indexContainer.ToRecords(codec.StringTable, codec.HuffmanTree, codec.CompressedData,
                codec.Header.MaxValueLength);
        }

        public void Write(BinaryWriter output, Codec codec)
        {
            MemoryStream bufferStream = new();
            BinaryWriter buffer = new(bufferStream);

            BitArray compressedData = new(codec.HuffmanTree.Encoder.TotalBits);

            var indexContainer = DataStructures.IndexContainer.FromRecords(Container, codec.StringTable,
                codec.HuffmanTree.Encoder, compressedData);
            int expectedLength = indexContainer.TotalSize();
            indexContainer.Write(buffer);

            byte[] indexData = bufferStream.ToArray();
            var data = new byte[(compressedData.Length - 1) / 8 + 1];
            compressedData.CopyTo(data, 0);

            codec.Header.IndexLength = (uint) indexData.Length;
            codec.Header.DataLength = (uint) data.Length;

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
            StringBuilder output = new();

            output.AppendLine("Nothing here yet");

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = $"{rootName}.data.txt";
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }
    }
}