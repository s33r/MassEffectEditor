using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aaron.MassEffectEditor.Core;
using Newtonsoft.Json;

namespace Aaron.MassEffectEditor.Coalesced
{
    public static class AnnotationCollection
    {


        private class AnnotationJson
        {
            public List<Annotation> Annotations { get; set; }
            public Games Game { get; set; }
        }


        public static string Serialize(Games game, List<Annotation> annotations)
        {
            AnnotationJson jsonObject = new()
            {
                Game = game,
                Annotations = annotations.Select(a => a.GetWireVersion()).ToList(),
            };

            JsonSerializerSettings settings = new()
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            return JsonConvert.SerializeObject(jsonObject, settings);
        }

        public static List<Annotation> Deserialze(string json)
        {
            AnnotationJson jsonObject = JsonConvert.DeserializeObject<AnnotationJson>(json);

            jsonObject.Annotations.ForEach(a => a.Game = jsonObject.Game);

            return jsonObject.Annotations;
        }

    }
}
