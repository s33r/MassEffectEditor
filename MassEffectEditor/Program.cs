using System.Collections.Generic;
using Aaron.MassEffectEditor.Coalesced;
using Aaron.MassEffectEditor.Core;
using System.IO;
using Aaron.MassEffectEditor.Coalesced.Records;
using Newtonsoft.Json;

namespace MassEffectEditor
{
    class Program
    {


        static void Main(string[] args)
        {
            Configuration.Instance.Initialize();

            string m1Location = Path.Join(Configuration.Instance.WorkingLocation, "me1.annotations.json");
            string m2Location = Path.Join(Configuration.Instance.WorkingLocation, "me2.annotations.json");
            string m3Location = Path.Join(Configuration.Instance.WorkingLocation, "me3.annotations.json");


            Container me1 = CoalescedFile.Load(Games.Me1,
                Configuration.Instance.Game[Games.Me1].CoalescedConfigurationLocation);
            Container me2 = CoalescedFile.Load(Games.Me2,
                Configuration.Instance.Game[Games.Me2].CoalescedConfigurationLocation);
            Container me3 = CoalescedFile.Load(Games.Me3,
                Configuration.Instance.Game[Games.Me3].CoalescedConfigurationLocation);

            BuildAnnotations(Games.Me1, me1, m1Location);
            BuildAnnotations(Games.Me2, me2, m2Location);
            BuildAnnotations(Games.Me3, me3, m3Location);


            List<Annotation> me1a = LoadAnnotations(m1Location);
            List<Annotation> me2a = LoadAnnotations(m2Location);
            List<Annotation> me3a = LoadAnnotations(m3Location);
        }

        static List<Annotation> LoadAnnotations(string inputLocation)
        {
            string json = File.ReadAllText(inputLocation);

            return AnnotationCollection.Deserialze(json);
        }

        static void BuildAnnotations(Games game, Container container, string outputLocation)
        {
            List<Annotation> annotations = new();

            foreach (FileRecord fileRecord in container.Files)
            {
                annotations.Add(new(game, fileRecord));

                foreach (SectionRecord sectionRecord in fileRecord)    
                {
                    annotations.Add(new(game, sectionRecord));
                    foreach (EntryRecord entryRecord in sectionRecord)
                    {
                        annotations.Add(new(game, entryRecord));
                    }
                }
            }


            string json = AnnotationCollection.Serialize(game, annotations);
            File.WriteAllText(outputLocation, json);
        }
  
    }
}
