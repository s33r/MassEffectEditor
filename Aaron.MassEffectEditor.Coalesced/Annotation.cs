using System.Runtime.CompilerServices;
using Aaron.MassEffectEditor.Coalesced.Records;
using Aaron.MassEffectEditor.Core;
using Newtonsoft.Json;

namespace Aaron.MassEffectEditor.Coalesced
{

    public enum EditorTypes
    {
        None,
        Checkbox,
        TextInput,
        MultilineInput,
        Integer,
        Decimal,
    }

    public class Annotation
    {
        public Games? Game { get; set; }
        public string FileName { get; set; }
        public string SectionName { get; set; }
        public string EntryName { get; set; }
        private EditorTypes? EditorType { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }


        public Annotation GetWireVersion()
        {
            return new()
            {
                Game = null,
                FileName = string.IsNullOrEmpty(FileName) ? null : FileName,
                SectionName = string.IsNullOrEmpty(SectionName) ? null : SectionName,
                EntryName = string.IsNullOrEmpty(EntryName) ? null : EntryName,
                EditorType = EditorType == EditorTypes.None ? null : EditorType,
                ShortDescription = string.IsNullOrEmpty(ShortDescription) ? null : ShortDescription,
                LongDescription = string.IsNullOrEmpty(LongDescription) ? null : LongDescription,
            };
        }

        public Annotation(Games game)
        {
            Game = game;
            EditorType = EditorTypes.None;
        }

        public Annotation()
            : this(Games.Unknown) { }

        public Annotation(Games game, FileRecord fileRecord)
            : this(game)
        {
            Game = game;
            FileName = fileRecord.Name;
        }

        public Annotation(FileRecord fileRecord)
            : this(Games.Unknown, fileRecord) { }

        public Annotation(Games game, SectionRecord sectionRecord)
            :this(game)
        {
            FileName = sectionRecord.Parent.Name;
            SectionName = sectionRecord.Name;
        }

        public Annotation(SectionRecord sectionRecord)
            : this(Games.Unknown, sectionRecord) { }

        public Annotation(Games game, EntryRecord entryRecord)
            :this(game)
        {
            Game = game;
            FileName = entryRecord.Parent.Parent.Name;
            SectionName = entryRecord.Parent.Name;
            EntryName = entryRecord.Name;
        }

        public Annotation(EntryRecord entryRecord)
            : this(Games.Unknown, entryRecord) { }


       
    }
}