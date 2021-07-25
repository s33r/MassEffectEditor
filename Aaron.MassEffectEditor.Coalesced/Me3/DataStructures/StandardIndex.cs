using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Me3.DataStructures
{
    class StandardIndex
    {
        public ushort Count { get; set; }
        public StandardIndexEntry[] Table { get; set; }

        public StandardIndexEntry this[int index]
        {
            get => Table[index];
            set => Table[index] = value;
        }

        public StandardIndex() { }

        public StandardIndex(ushort count)
        {
            Table = new StandardIndexEntry[count];
            Count = count;

            for (int i = 0; i < count; i++)
            {
                Table[i] = new();
            }
        }

        public ushort Size()
        {
            return Size(Count);
        }

        public static ushort Size(int count)
        {
            return (ushort)(2 + (count * StandardIndexEntry.Size()));
        }


        public void Read(BinaryReader reader)
        {
            Count = reader.ReadUInt16();
            Table = new StandardIndexEntry[Count];

            for (int i = 0; i < Count; i++)
            {
                Table[i] = new StandardIndexEntry();
                Table[i].Read(reader);
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(Count);

            foreach (var entry in Table)
            {
                entry.Write(writer);
            }
        }


        public override string ToString()
        {
            return $"{Count} ({Size()} bytes)";
        }

    }
}
