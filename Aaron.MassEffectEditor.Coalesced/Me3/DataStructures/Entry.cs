using System.IO;

namespace Aaron.MassEffectEditor.Coalesced.Me3.DataStructures
{
    internal class Entry
    {
        public Item[] Items;
        public StandardIndexEntry Parent { get; set; }
        public StandardIndex Index { get; set; }

        public Entry() { }

        public Entry(ushort count, StandardIndexEntry parent)
        {
            Parent = parent;
            Index = new StandardIndex(count);
            Items = new Item[count];

            for (int i = 0; i < count; i++)
            {
                Items[i] = new Item();
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

            Items = new Item[Index.Count];

            for (int i = 0; i < Index.Count; i++)
            {
                uint newOrigin = origin + Index[i].Offset;
                reader.BaseStream.Seek(newOrigin, SeekOrigin.Begin);

                Items[i] = new Item();
                Items[i].Read(reader, Index[i]);
            }
        }

        public void Write(BinaryWriter writer)
        {
            Index.Write(writer);

            foreach (Item itemIndex in Items)
            {
                itemIndex.Write(writer);
            }
        }
    }
}