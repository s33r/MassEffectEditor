using Aaron.MassEffectEditor.Coalesced.Records;
using Aaron.MassEffectEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Aaron.MassEffectEditor.Coalesced
{
    public class Container : IEnumerable<IRecord>
    {
        public List<FileRecord> Files { get; }

        public Container()
        {
            Files = new List<FileRecord>();
        }
        public Container(int count)
        {
            Files = Utility.CreateList<FileRecord>(count).ToList();
        }

        public Container(IEnumerable<FileRecord> records)
        {
            Files = new List<FileRecord>(records);
        }

        public int RecordCount => GetEntries().Count();


        public void Sort(Comparison<IRecord> comparison)
        {
            Files.Sort(comparison);

            foreach (FileRecord fileRecord in Files)
            {
                fileRecord.Sort(comparison);

                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    sectionRecord.Sort(comparison);
                }
            }
        }



        public IEnumerator<IRecord> GetEnumerator()
        {
            foreach (FileRecord fileRecord in Files)
            {
                yield return fileRecord;

                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    yield return sectionRecord;

                    foreach (EntryRecord entryRecord in sectionRecord)
                    {
                        yield return entryRecord;
                    }
                }
            }
        }

        public IEnumerable<FileRecord> GetFiles()
        {
            foreach (FileRecord fileRecord in Files)
            {
                yield return fileRecord;
            }
        }

        public IEnumerable<SectionRecord> GetSections()
        {
            foreach (FileRecord fileRecord in Files)
            {             
                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    yield return sectionRecord;
                }
            }
        }

        public IEnumerable<EntryRecord> GetEntries()
        {
            foreach (FileRecord fileRecord in Files)
            {
                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    foreach (EntryRecord entryRecord in sectionRecord)
                    {
                        yield return entryRecord;
                    }
                }
            }
        }

        public IEnumerable<string> GetItems()
        {
            foreach (FileRecord fileRecord in Files)
            {
                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    foreach (EntryRecord entryRecord in sectionRecord)
                    {
                        foreach(string item in entryRecord)
                        {
                            yield return item;
                        }
                    }
                }
            }
        }

        public string GetData()
        {
            int longestLength;

            return GetData(out longestLength);
        }

        public string GetData(out int longestLength)
        {
            int longest = 0;
            StringBuilder dataBuffer = new();

            foreach (string item in GetItems())
            {
                dataBuffer.Append(item + '\0');

                if(item.Length > longest)
                {
                    longest = item.Length;
                }
            }

            longestLength = longest;
            return dataBuffer.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string DumpRecords()
        {
            StringBuilder output = new();

            foreach (FileRecord fileRecord in Files)
            {
                output.AppendLine($"{fileRecord.Name}");

                foreach (SectionRecord sectionRecord in fileRecord)
                {
                    output.AppendLine($"    {sectionRecord.Name}");

                    foreach (EntryRecord entryRecord in sectionRecord)
                    {
                        output.AppendLine($"        {entryRecord.Name}");

                        int valueIndex = 0;
                        foreach (string value in entryRecord)
                        {
                            output.AppendLine($"            [{valueIndex++}] {value}");
                        }
                    }
                }
            }

            return output.ToString();
        }

        public void DumpRecords(string fileName)
        {
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string result = DumpRecords();

            File.WriteAllText(outputLocation, result);
        }
    }
}
