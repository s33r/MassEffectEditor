using Aaron.MassEffectEditor.Core;
using Aaron.MassEffectEditor.Core.Compression.Huffman;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Text = System.Text;

namespace Aaron.MassEffectEditor.Coalesced.Me3
{
    class HuffmanTreeBlock : IBlock<Codec>
    {
        public const int HUFFMAN_TUPLE_SIZE = 8;
        public List<Pair> HuffmanTuples { get; private set; }

        public Encoder Encoder { get; private set; }

        public void Read(byte[] data, Codec codec)
        {
            BinaryReader input = new(new MemoryStream(data));

            ushort entryCount = input.ReadUInt16();

            List<Pair> pairs = new();

            for (int currentEntry = 0; currentEntry < entryCount; currentEntry++)
            {
                Pair pair = new()
                {
                    Left = input.ReadInt32(),
                    Right = input.ReadInt32(),
                };

                pairs.Add(pair);
            }

            HuffmanTuples = pairs;
        }

        public void Validate(Codec codec) { }

        public void Write(BinaryWriter output, Codec codec)
        {
            string data = codec.Container.GetData(out int maxValueLength);
            codec.Header.MaxValueLength = maxValueLength;

            Encoder = new Encoder();
            Encoder.Build(data);

            HuffmanTuples = Encoder.GetPairs().ToList();
            codec.Header.HuffmanLength = (uint) (HUFFMAN_TUPLE_SIZE * HuffmanTuples.Count) + 2;

            output.Write((ushort) HuffmanTuples.Count);

            foreach (Pair pair in HuffmanTuples)
            {
                output.Write(pair.Left);
                output.Write(pair.Right);
            }
        }

        public string Dump()
        {
            Text.StringBuilder output = new();

            output.AppendLine($"Count = {HuffmanTuples.Count}");

            foreach (var tuple in HuffmanTuples)
            {
                output.AppendLine($"({tuple.Left,8}, {tuple.Right,8})");
            }

            return output.ToString();
        }

        public void Dump(string rootName)
        {
            string fileName = $"{rootName}.huffman.txt";
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }
    }
}