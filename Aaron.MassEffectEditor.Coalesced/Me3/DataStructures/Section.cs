using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Me3.DataStructures
{
    class Section
    {

        public StandardIndexEntry Parent { get; set; }
        public StandardIndex Index { get; set; }

        public Entry[] Entries;


        public Section() { }

        public Section(ushort count, StandardIndexEntry parent)
        {
            Parent = parent;

            Index = new(count);
            Entries = new Entry[count];

            for (int i = 0; i < count; i++)
            {
                Entries[i] = new Entry();
            }
        }

        public ushort Size()
        {
            return Size(Index.Count);
        }

        public static ushort Size(int count)
        {
            return StandardIndex.Size(count);
        }

        public void Read(BinaryReader reader, uint origin, StandardIndexEntry parent)
        {
            Parent = parent;
            
            Index = new StandardIndex();
            Index.Read(reader);

            Entries = new Entry[Index.Count];

            for (int i = 0; i < Index.Count; i++)
            {
                uint newOrigin = origin + Index[i].Offset;
                reader.BaseStream.Seek(newOrigin, SeekOrigin.Begin);

                Entries[i] = new Entry();
                Entries[i].Read(reader, newOrigin, Index[i]);
            }
        }

        public void Write(BinaryWriter writer)
        {
            Index.Write(writer);

            foreach (Entry entryIndex in Entries)
            {
                entryIndex.Write(writer);
            }
        }
    }
}
