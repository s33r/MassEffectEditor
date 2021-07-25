using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Me3.DataStructures
{
    class StandardIndexEntry
    {
        public ushort StringTableIndex { get; set; }
        public uint Offset { get; set; }

        public StandardIndexEntry() { }

        public StandardIndexEntry(ushort stringTableIndex)
        {
            StringTableIndex = stringTableIndex;
        }

        public StandardIndexEntry(ushort stringTableIndex, uint offset)
        {
            StringTableIndex = stringTableIndex;
            Offset = offset;
        }

        public static int Size()
        {
            return 6;
        }

        public string GetString(StringTableBlock stringTable)
        {
            return stringTable.Entries[StringTableIndex].Value;
        }

        public void Read(BinaryReader reader)
        {
            StringTableIndex = reader.ReadUInt16();
            Offset = reader.ReadUInt32();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(StringTableIndex);
            writer.Write(Offset);
        }

        public override string ToString()
        {
            return string.Format("{0} | {1}", Offset, StringTableIndex);
        }
    }
}
