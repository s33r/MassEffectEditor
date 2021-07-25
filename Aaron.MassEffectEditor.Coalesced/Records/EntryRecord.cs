using Aaron.MassEffectEditor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public class EntryRecord 
        : IRecord, IEquatable<IRecord>, IEnumerable<string>, IReadOnlyList<string>
    {
        public string Name { get; set; }

        private List<string> values;

        public IRecord Parent { get; internal set; }

        public string Path
        {
            get
            {
                return Parent.Parent.Name + '/' + Parent.Name + '/' + Name;
            }
        }

        public int Count { get { return values.Count; } }

        public string this[int index]
        {
            get
            {
                return values[index];
            }
            set
            {
                values[index] = value;
            }
        }

        public EntryRecord(List<string> items, string name)
        {
            Name = name;
            values = new List<string>(items);

        }

        public EntryRecord(List<string> items)
          : this(items, null) { }

        public EntryRecord()
            : this(new List<string>()) { }

        public EntryRecord(string name)
            : this(new List<string>(), name) { }



        public void SetValues(IEnumerable<string> items)
        {
            values = new List<string>(items);
        }

        public override string ToString()
        {
            return string.Format("EntryRecord [{0} Values] {1}", values.Count, Name);
        }

        public bool Equals(IRecord other)
        {
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

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }


    }
}
