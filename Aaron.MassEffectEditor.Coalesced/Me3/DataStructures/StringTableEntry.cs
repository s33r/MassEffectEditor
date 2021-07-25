using Aaron.MassEffectEditor.Core.Checksums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Me3.DataStructures
{
    internal class StringTableEntry : IEquatable<StringTableEntry>
    {
        public string Value { get; set; }
        public uint Offset { get; set; }
        public uint Checksum { get; set; }

        public bool Validate()
        {
            uint crc32 = Crc32.Compute(Value);

            return crc32 == Checksum;
        }

        public override string ToString()
        {
            return $"[Offset={Offset}][Checksum={Checksum:X}] {Value}";
        }

        public StringTableEntry()
        {

        }

        public StringTableEntry(string value, uint offset, uint checksum)
        {
            Value = value;
            Offset = offset;
            Checksum = checksum;
        }

        public StringTableEntry(string value, uint offset)
        {
            Value = value;
            Offset = offset;
            Checksum = Crc32.Compute(Value);
        }

        public StringTableEntry(string value)
        {
            Value = value;
            Checksum = Crc32.Compute(Value);
        }

        public bool Equals(StringTableEntry other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Checksum == Checksum;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((StringTableEntry)obj);
        }

        public override int GetHashCode()
        {
            return (int)Crc32.Compute(Value);
        }
    }
}
