using Aaron.MassEffectEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public class FileRecord
        : IRecord, IEquatable<IRecord>, IList<SectionRecord>
    {
        private List<SectionRecord> _values;

        public string FriendlyName =>
            System.IO.Path.GetFileName(Name.Replace("\\",
                "/")); //Windows can use either slash, but all others need unix style.

        public FileRecord(List<SectionRecord> sections, string name)
        {
            Name = name;
            _values = new List<SectionRecord>();

            foreach (SectionRecord sectionRecord in sections)
            {
                sectionRecord.Parent = this;
                _values.Add(sectionRecord);
            }
        }

        public FileRecord(List<SectionRecord> sections)
            : this(sections, null) { }

        public FileRecord(int count)
            : this(Utility.CreateList<SectionRecord>(count).ToList()) { }

        public FileRecord()
            : this(new List<SectionRecord>()) { }

        public FileRecord(string name)
            : this(new List<SectionRecord>(), name) { }

        public bool Equals(IRecord other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Name == Name;
        }

        public int Count => _values.Count;

        public bool IsReadOnly => false;

        public SectionRecord this[int index]
        {
            get => _values[index];
            set
            {
                value.Parent = this;
                _values[index] = value;
            }
        }

        IEnumerator<SectionRecord> IEnumerable<SectionRecord>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public int IndexOf(SectionRecord item)
        {
            return _values.IndexOf(item);
        }

        public void Insert(int index, SectionRecord item)
        {
            item.Parent = this;
            _values.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _values[index].Parent = null;
            _values.RemoveAt(index);
        }

        public void Add(SectionRecord item)
        {
            item.Parent = this;
            _values.Add(item);
        }

        public void Clear()
        {
            foreach (SectionRecord item in _values)
            {
                item.Parent = null;
            }

            _values.Clear();
        }

        public bool Contains(SectionRecord item)
        {
            return _values.Contains(item);
        }

        public void CopyTo(SectionRecord[] array, int arrayIndex)
        {
            _values.CopyTo(array, arrayIndex);
        }

        public bool Remove(SectionRecord item)
        {
            item.Parent = null;
            return _values.Remove(item);
        }

        public string Name { get; set; }
        public string Path => FriendlyName;

        public IRecord Parent => null;

        public IEnumerator GetEnumerator()
        {
            return _values.GetEnumerator();
        }


        public void SetValues(IEnumerable<SectionRecord> sections)
        {
            _values = new List<SectionRecord>();

            foreach (SectionRecord section in sections)
            {
                section.Parent = this;
                _values.Add(section);
            }
        }

        public override string ToString()
        {
            return $"FileRecord [{_values.Count} Values] {Name}";
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