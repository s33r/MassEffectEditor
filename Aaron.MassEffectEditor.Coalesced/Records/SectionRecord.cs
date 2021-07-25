using Aaron.MassEffectEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public class SectionRecord 
        : IRecord, IEquatable<IRecord>, IReadOnlyList<EntryRecord>
    {
        public string Name { get; set; }

        public IRecord Parent { get; internal set; }

        public int Count { get { return values.Count; } }

        public string Path
        {
            get
            {
                return Parent.Name + '/' + Name;
            }
        }

        private List<EntryRecord> values;

        public EntryRecord this[int index]
        {
            get
            {
                return values[index];
            }
            set
            {
                value.Parent = this;
                values[index] = value;
            }
        }

        public SectionRecord(List<EntryRecord> entries, string name)
        {
            Name = name;
            values = new List<EntryRecord>();

            foreach (EntryRecord entryRecord in entries)
            {
                entryRecord.Parent = this;
                values.Add(entryRecord);
            }
        }

        public SectionRecord(List<EntryRecord> sections)
          : this(sections, null) { }

        public SectionRecord(int count)
            : this(Utility.CreateList<EntryRecord>(count).ToList()) { }

        public SectionRecord()
            : this(new List<EntryRecord>()) { }

        public SectionRecord(string name)
            : this(new List<EntryRecord>(), name) { }


        public void SetValues(IEnumerable<EntryRecord> entries)
        {
            values = new List<EntryRecord>();

            foreach (EntryRecord entry in entries)
            {
                entry.Parent = this;
                values.Add(entry);
            }
        }

        public override string ToString()
        {
            return string.Format("SectionRecord [{0} Values] {1}", values.Count, Name);
        }

        public bool Equals(IRecord other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Name == Name;
        }

        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            return Equals((IRecord)other);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public IEnumerator GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator<EntryRecord> IEnumerable<EntryRecord>.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public void Sort(Comparison<IRecord> comparer)
        {
            values.Sort(comparer);
        }
    }
}
