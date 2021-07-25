using Aaron.MassEffectEditor.Core;
using Aaron.MassEffectEditor.Core.Compression.Huffman;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Text = System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Me3
{
    class HuffmanTreeBlock : IBlock<Codec>
    {

        public const int HUFFMAN_TUPLE_SIZE = 8;
        public List<Pair> HuffmanTuples { get; private set; }

        public Encoder Encoder { get; private set; }

        public void Read(byte[] data, Codec codec)
        {
            BinaryReader input = new BinaryReader(new MemoryStream(data));

            ushort entryCount = input.ReadUInt16();

            List<Pair> pairs = new List<Pair>();

            for (int currentEntry = 0; currentEntry < entryCount; currentEntry++)
            {
                Pair pair = new Pair();
                pair.Left = input.ReadInt32();
                pair.Right = input.ReadInt32();

                pairs.Add(pair);
            }

            HuffmanTuples = pairs;
        }

        public void Validate(Codec codec)
        {

        }

        public void Write(BinaryWriter output, Codec codec)
        {
            string data = codec.Container.GetData(out int maxValueLength);
            codec.Header.MaxValueLength = maxValueLength;

            Encoder = new Encoder();
            Encoder.Build(data);

            HuffmanTuples = Encoder.GetPairs().ToList();
            codec.Header.HuffmanLength = (uint)(HUFFMAN_TUPLE_SIZE * HuffmanTuples.Count) + 2;

            output.Write((ushort)HuffmanTuples.Count);

            foreach(Pair pair in HuffmanTuples)
            {
                output.Write(pair.Left);
                output.Write(pair.Right);
            }
        }

        public string Dump()
        {
            Text.StringBuilder output = new Text.StringBuilder();

            output.AppendLine(string.Format("Count = {0}", HuffmanTuples.Count));

            foreach(var tuple in HuffmanTuples)
            {
                output.AppendLine(string.Format("({0,8}, {1,8})", tuple.Left, tuple.Right));
            }

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = string.Format("{0}.huffman.txt", rootName);
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }
    }
}
