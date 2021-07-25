using Aaron.MassEffectEditor.Coalesced.Me3;
using Aaron.MassEffectEditor.Coalesced.Records;
using Aaron.MassEffectEditor.Core;
using Aaron.MassEffectEditor.Core.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced
{
    public static class CoalescedFile
    {
        private static Dictionary<Games, ICodec> codecs = new Dictionary<Games, ICodec>();

        static CoalescedFile()
        {
            codecs.Add(Games.Me3, new Me3.Codec());
        }


        public static Container Load(Games game, byte[] data)
        {
            if(!codecs.ContainsKey(game))
            {
                throw new GameNotSupportedException(game);
            }

            Codec codec = (Codec)codecs[game];
            Container container = codec.Decode(data);

            return container;
        }

        public static Container Load(Games game, string fileLocation)
        {
            byte[] data = File.ReadAllBytes(fileLocation);
            
            return Load(game, data);
        }

        public static byte[] Save(Games game, Container container)
        {
            if (!codecs.ContainsKey(game))
            {
                throw new GameNotSupportedException(game);
            }

            Codec codec = (Codec)codecs[game];
            byte[] data = codec.Encode(container);

            return data;
        }

        public static void Save(Games game, Container container, string outputLocation)
        {
            byte[] data = Save(game, container);

            File.WriteAllBytes(outputLocation, data);
        }

        public static void Compare(byte[] oldData, byte[] newData, byte[] gibbedData)
        {

            //bool byteMatch = CompareBytes(oldData, newData);

            Codec oCodec = new Codec("old");
            Codec nCodec = new Codec("new");
            Codec gCodec = new Codec("gib");

            Container oContainer = oCodec.Decode(oldData);
            Container nContainer = nCodec.Decode(newData);
            Container gContainer = gCodec.Decode(gibbedData);

            oCodec.Dump();
            nCodec.Dump();
            gCodec.Dump();

            oContainer.DumpRecords("old.container.txt");
            nContainer.DumpRecords("new.container.txt");
            gContainer.DumpRecords("gib.container.txt");
        }

        private static bool CompareBytes(byte[] oldData, byte[] newData)
        {
            if (oldData.Length != newData.Length)
            {
                return false;
            }

            for (int index = 0; index < oldData.Length; index++)
            {
                byte originalByte = oldData[index];
                byte newByte = newData[index];

                if (originalByte != newByte)
                {
                    Console.WriteLine("[{0}] {1:X2} | {2:X2}", index, originalByte, newByte);
                    return false;
                }
            }

            return true;
        }



        private static void dumpStringTable(Codec codec, StreamWriter output)
        {
            foreach (var entry in codec.StringTable.Entries)
            {
                output.WriteLine("[{0,8}] ({1,10}) {2}", entry.Offset, entry.Checksum, entry.Value);
            }
        }

        

        
    }
}
