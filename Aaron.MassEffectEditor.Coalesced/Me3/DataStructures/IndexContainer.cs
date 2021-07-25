using Aaron.MassEffectEditor.Coalesced.Records;
using Aaron.MassEffectEditor.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Huffman = Aaron.MassEffectEditor.Core.Compression.Huffman;

namespace Aaron.MassEffectEditor.Coalesced.Me3.DataStructures
{
    class IndexContainer
    {
        public StandardIndex Index { get; set; }

        public Section[] Sections { get; set; }

        public IndexContainer() { }

        public IndexContainer(ushort count)
        {
            Index = new StandardIndex(count);
            Sections = new Section[count];

            for (int i = 0; i < count; i++)
            {
                Sections[i] = new Section();
            }
        }

        public int TotalSize()
        {
            int totalSize = Size();

            foreach (Section sectionIndex in Sections)
            {
                totalSize += sectionIndex.Size();

                foreach (Entry entryIndex in sectionIndex.Entries)
                {
                    totalSize += entryIndex.Size();

                    foreach (Item itemIndex in entryIndex.Items)
                    {
                        totalSize += itemIndex.Size();
                    }
                }
            }

            return totalSize;
        }

        public int Size()
        {
            return Index.Size();
        }

        public static int Size(int count)
        {
            return StandardIndex.Size(count);
        }

        public void Read(BinaryReader reader)
        {
            Index = new StandardIndex();
            Index.Read(reader);

            Sections = new Section[Index.Count];

            for (int i = 0; i < Index.Count; i++)
            {
                reader.BaseStream.Seek(Index[i].Offset, SeekOrigin.Begin);

                Sections[i] = new Section();
                Sections[i].Read(reader, Index[i].Offset, Index[i]);
            }
        }

        public void Write(BinaryWriter writer)
        {
            Index.Write(writer);

            foreach (Section sectionIndex in Sections)
            {
                sectionIndex.Write(writer);
            }
        }

        public Container ToRecords(StringTableBlock stringTable, HuffmanTreeBlock huffmanTree, BitArray compressedData, int maxValueLength)
        {
            List<FileRecord> fileRecords = Index.Table
                .Select(f => new FileRecord(f.GetString(stringTable)))
                .ToList();

            for (int sectionIndex = 0; sectionIndex < Sections.Length; sectionIndex++)
            {
                Section currentSection = Sections[sectionIndex];

                fileRecords[sectionIndex]
                    .SetValues(currentSection.Index.Table
                    .Select(s => new SectionRecord(s.GetString(stringTable)))
                    .ToList());                

                for (int entryIndex = 0; entryIndex < currentSection.Entries.Length; entryIndex++)
                {
                    Entry currentEntry = currentSection.Entries[entryIndex];

                    fileRecords[sectionIndex][entryIndex]
                        .SetValues(currentEntry.Index.Table
                        .Select(e => new EntryRecord(e.GetString(stringTable)))
                        .ToList());

                    for (int itemIndex = 0; itemIndex < currentEntry.Items.Length; itemIndex++)
                    {
                        Item currentItem = currentEntry.Items[itemIndex];

                        fileRecords[sectionIndex][entryIndex][itemIndex]
                            .SetValues(currentItem.Decode(huffmanTree, compressedData, maxValueLength));
                    }
                }
            }

            Container container = new(fileRecords);
            //container.Sort();

            return container;
        }

        public static IndexContainer FromRecords(Container container, StringTableBlock stringTable, Huffman.Encoder encoder, BitArray compressedData)
        {
            container.Sort((x, y) => SortByIndexComparer(x, y, stringTable));

            IndexContainer indexContainer = new((ushort)container.Files.Count);

            int bitOffset = 0;
            uint fileOffset = (ushort)indexContainer.Size();

            for (int sectionIndex = 0; sectionIndex < indexContainer.Sections.Length; sectionIndex++)
            {                 
                FileRecord currentFileRecord = container.Files[sectionIndex];
                Section currentSection = new((ushort)currentFileRecord.Count, indexContainer.Index.Table[sectionIndex]);
                indexContainer.Sections[sectionIndex] = currentSection;

                currentSection.Parent.Offset = fileOffset;
                currentSection.Parent.StringTableIndex = stringTable.IndexOf(currentFileRecord.Name);

                uint sectionOffset = currentSection.Size();

                for (int entryIndex = 0; entryIndex < currentSection.Entries.Length; entryIndex++)
                {
                    SectionRecord currentSectionRecord = container.Files[sectionIndex][entryIndex];
                    Entry currentEntry = new((ushort)currentSectionRecord.Count, currentSection.Index.Table[entryIndex]);
                    currentSection.Entries[entryIndex] = currentEntry;

                    currentEntry.Parent.Offset = sectionOffset;
                    currentEntry.Parent.StringTableIndex = stringTable.IndexOf(currentSectionRecord.Name);

                    ushort entryOffset = currentEntry.Size();

                    for (int itemIndex = 0; itemIndex < currentEntry.Items.Length; itemIndex++)
                    {
                        EntryRecord currentEntryRecord = container.Files[sectionIndex][entryIndex][itemIndex];
                        Item currentItem = new((ushort)currentSectionRecord[itemIndex].Count, currentEntry.Index.Table[itemIndex]);
                        currentEntry.Items[itemIndex] = currentItem;

                        currentItem.Parent.Offset = entryOffset;
                        currentItem.Parent.StringTableIndex = stringTable.IndexOf(currentEntryRecord.Name);

                        for (int valueIndex = 0; valueIndex < currentItem.Count; valueIndex++)
                        {
                            string currentValue = currentEntryRecord[valueIndex];
                            bitOffset = currentItem.Encode(currentValue, valueIndex, bitOffset, encoder, compressedData);
                        }

                        entryOffset += currentItem.Size();
                    }

                    sectionOffset += entryOffset;
                }

                fileOffset += sectionOffset;
            }


            return indexContainer;
        }

        private static int SortByIndexComparer(IRecord x, IRecord y, StringTableBlock stringTable)
        {
            return stringTable.IndexOf(x.Name).CompareTo(stringTable.IndexOf(y.Name));
        }

        public string Dump()
        {
            StringBuilder output = new();

            output.AppendLine($"Expected Size = {TotalSize()}");

            foreach (StandardIndexEntry fileIndexEntry in Index.Table)
            {
                output.AppendLine($"{fileIndexEntry.Offset,10} | {fileIndexEntry.StringTableIndex}");
            }

            output.AppendLine();

            foreach (Section section in Sections)
            {
                output.AppendLine($"Section ({section.Index.Count} entries / {section.Size()} bytes)");
                foreach(Entry entry in section.Entries)
                {
                    output.AppendLine($"    Entry ({entry.Index.Count} entries / {entry.Size()} bytes)");
                    foreach (Item item in entry.Items)
                    {
                        output.AppendLine($"        Item ({item.Count} entries / {item.Size()} bytes)");                        
                        foreach (int value in item.Values)
                        {
                            //output.AppendLine(value.ToString());
                        }
                        //output.AppendLine();
                    }
                }
            }

            return output.ToString();
        }

        public void Dump(string rootName)
        {

            string fileName = $"{rootName}.index.txt";
            string outputLocation = Path.Join(Configuration.Instance.WorkingLocation, fileName);

            string text = Dump();
            text = rootName + "\n" + text;

            File.WriteAllText(outputLocation, text);
        }

    }
}
