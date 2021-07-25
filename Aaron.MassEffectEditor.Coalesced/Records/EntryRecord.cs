using System;
using System.Collections.Generic;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public class EntryRecord
        : IRecord, IEquatable<IRecord>, IEnumerable<string>, IReadOnlyList<string>
    {
        private List<string> _values;

        public EntryRecord(List<string> items, string name)
        {
            Name = name;
            _values = new List<string>(items);
        }

        public EntryRecord(List<string> items)
            : this(items, null) { }

        public EntryRecord()
            : this(new List<string>()) { }

        public EntryRecord(string name)
            : this(new List<string>(), name) { }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        public bool Equals(IRecord other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Name == Name;
        }

        public int Count => _values.Count;

        public string this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public string Name { get; set; }

        public IRecord Parent { get; internal set; }

        public string Path => Parent.Parent.Name + '/' + Parent.Name + '/' + Name;

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }


        public void SetValues(IEnumerable<string> items)
        {
            _values = new List<string>(items);
        }

        public override string ToString()
        {
            return $"EntryRecord [{_values.Count} Values] {Name}";
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
    }
}