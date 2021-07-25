using Aaron.MassEffectEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public class FileRecord
        : IRecord, IEquatable<IRecord>, IList<SectionRecord>
    {
        public string Name { get; set; }
        public string FriendlyName
        {
            get
            {
                return System.IO.Path.GetFileName(Name.Replace("\\", "/")); //Windows can use either slash, but all others need unix style.
            }
        }
        public string Path { get { return FriendlyName; } }

        public IRecord Parent { get { return null; } }

        public int Count { get { return values.Count; } }

        public bool IsReadOnly { get { return false; } }

        private List<SectionRecord> values;

        public SectionRecord this[int index]
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

        public FileRecord(List<SectionRecord> sections, string name)
        {
            Name = name;
            values = new List<SectionRecord>();

            foreach (SectionRecord sectionRecord in sections)
            {
                sectionRecord.Parent = this;
                values.Add(sectionRecord);
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


        public void SetValues(IEnumerable<SectionRecord> sections)
        {
            values = new List<SectionRecord>();

            foreach (SectionRecord section in sections)
            {
                section.Parent = this;
                values.Add(section);
            }
        }

        public override string ToString()
        {
            return string.Format("FileRecord [{0} Values] {1}", values.Count, Name);
        }

        public bool Equals(IRecord other)
        {
            if(other == null)
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

        IEnumerator<SectionRecord> IEnumerable<SectionRecord>.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        public int IndexOf(SectionRecord item)
        {
            return values.IndexOf(item);
        }

        public void Insert(int index, SectionRecord item)
        {
            item.Parent = this;
            values.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            values[index].Parent = null;
            values.RemoveAt(index);
        }

        public void Add(SectionRecord item)
        {
            item.Parent = this;
            values.Add(item);
        }

        public void Clear()
        {
            foreach (SectionRecord item in values)
            {
                item.Parent = null;
            }

            values.Clear();
        }

        public bool Contains(SectionRecord item)
        {
            return values.Contains(item);
        }

        public void CopyTo(SectionRecord[] array, int arrayIndex)
        {
            values.CopyTo(array, arrayIndex);
            
        }

        public bool Remove(SectionRecord item)
        {
            item.Parent = null;
            return Remove(item);
        }

        public void Sort(Comparison<IRecord> comparer)
        {
            values.Sort(comparer);
        }
    }
}
