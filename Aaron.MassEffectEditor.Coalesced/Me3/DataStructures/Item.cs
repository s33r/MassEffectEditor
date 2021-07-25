using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Huffman = Aaron.MassEffectEditor.Core.Compression.Huffman;
namespace Aaron.MassEffectEditor.Coalesced.Me3.DataStructures
{
    class Item
    {
        public StandardIndexEntry Parent;
        public ushort Count;
        public int[] Values;

        public Item() { }

        public Item(ushort count, StandardIndexEntry parent)
        {
            Parent = parent;
            Count = count;
            Values = new int[count];
        }

        public ushort Size()
        {
            return Size(Count);
        }

        public static ushort Size(int count)
        {
            return (ushort)(2 + (4 * count));
        }

        public string Decode(int index, HuffmanTreeBlock huffmanTree, BitArray compressedData, int maxValueLength)
        {
            int offset = Values[index];
            long type = (offset & 0xE0000000) >> 29;

            if (type == 1)
            {
                return null;
            }
            else if (type == 2)
            {
                offset &= 0x1FFFFFFF;
                string text = Huffman.Decoder.Decode(huffmanTree.HuffmanTuples.ToArray(), compressedData, offset, maxValueLength);

                return text;
            }

            throw new InvalidDataException("Unknown compression type");
        }

        public List<string> Decode(HuffmanTreeBlock huffmanTree, BitArray compressedData, int maxValueLength)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < Values.Length; i++)
            {
                result.Add(Decode(i, huffmanTree, compressedData, maxValueLength));
            }

            return result;
        }

        public int Encode(string item, int index, int bitOffset, Huffman.Encoder encoder, BitArray compressedData)
        {
            int value;
            int newBitOffset = bitOffset;
            

            if (item == null)
            {
                value = (1 << 29) | bitOffset;
            }
            else
            {
                value = (2 << 29) | bitOffset;
                newBitOffset += encoder.Encode(item + '\0', compressedData, bitOffset);
            }

            Values[index] = value;

            return newBitOffset;

        }

        public void Read(BinaryReader reader, StandardIndexEntry parent)
        {
            Parent = parent;
            Count = reader.ReadUInt16();
            Values = new int[Count];

            for (int i = 0; i < Count; i++)
            {
                Values[i] = reader.ReadInt32();
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Count);

            foreach(int value in Values)
            {
                writer.Write(value);
            }
        }
    }
}
