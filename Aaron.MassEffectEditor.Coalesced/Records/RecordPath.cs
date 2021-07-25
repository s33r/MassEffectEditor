using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public class RecordPath
    {

        public const char PATH_SEPERATOR = '/';

        public string FileName { get; set; }
        public string SectionName { get; set; }
        public string EntryName { get; set; }

        public override string ToString()
        {            
            if(FileName == null)
            {
                return string.Empty;
            }

            StringBuilder buffer = new StringBuilder();

            buffer.Append(FileName);

            if(SectionName != null)
            {
                buffer.Append(PATH_SEPERATOR + SectionName);

                if(EntryName != null)
                {
                    buffer.Append(PATH_SEPERATOR + EntryName);
                }
            }

            return buffer.ToString();
        }

        public static RecordPath FromString(string path)
        {
            if(path == null || path == string.Empty)
            {
                return new RecordPath();
            }

            string[] elements = path.Split(PATH_SEPERATOR);

            if(elements.Length == 0)
            {
                return new RecordPath();
            }

            RecordPath result = new RecordPath();
            result.EntryName = elements[0];

            if(elements.Length > 1)
            {
                result.SectionName = elements[1];

                if(elements.Length > 2)
                {
                    result.EntryName = elements[2];
                }
            }


            return result;
        }
    }
}
