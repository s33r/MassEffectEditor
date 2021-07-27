using Aaron.MassEffectEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public class SectionRecord
        : IRecord, IEquatable<IRecord>, IList<EntryRecord>
    {
        private List<EntryRecord> _values;

        public SectionRecord(List<EntryRecord> entries, string name)
        {
            Name = name;
            _values = new List<EntryRecord>();

            foreach (EntryRecord entryRecord in entries)
            {
                entryRecord.Parent = this;
                _values.Add(entryRecord);
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

        public bool Equals(IRecord other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Name == Name;
        }

        public void Add(EntryRecord item)
        {
            item.Parent = this;
            _values.Add(item);
        }

        public void Clear()
        {
            foreach (EntryRecord item in _values)
            {
                item.Parent = null;
            }

            _values.Clear();
        }

        public bool Contains(EntryRecord item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(EntryRecord[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public bool Remove(EntryRecord item)
        {
            item.Parent = null;
            return _values.Remove(item);
        }

        public int Count => _values.Count;
        public bool IsReadOnly => false;

        public int IndexOf(EntryRecord item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, EntryRecord item)
        {
            item.Parent = this;
            _values.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _values[index].Parent = null;
            _values.RemoveAt(index);
        }

        public EntryRecord this[int index]
        {
            get => _values[index];
            set
            {
                value.Parent = this;
                _values[index] = value;
            }
        }

        IEnumerator<EntryRecord> IEnumerable<EntryRecord>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public string Name { get; set; }

        public IRecord Parent { get; internal set; }

        public string Path => Parent.Name + '/' + Name;

        public IEnumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }


        public void SetValues(IEnumerable<EntryRecord> entries)
        {
            _values = new List<EntryRecord>();

            foreach (EntryRecord entry in entries)
            {
                entry.Parent = this;
                _values.Add(entry);
            }
        }

        public override string ToString()
        {
            return $"SectionRecord [{_values.Count} Values] {Name}";
        }

        public override bool Equals(object other)
        {
            if (other == null || GetType() != other.GetType())
            {
                return false;
            }

            return Equals((IRecord) other);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public void Sort(Comparison<IRecord> comparer)
        {
            _values.Sort(comparer);
        }
    }
}