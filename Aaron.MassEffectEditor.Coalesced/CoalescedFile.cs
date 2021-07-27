using Aaron.MassEffectEditor.Core;
using Aaron.MassEffectEditor.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aaron.MassEffectEditor.Coalesced
{
    public static class CoalescedFile
    {
        private delegate ICodec CodecFactory(string name);
        private static readonly Dictionary<Games, CodecFactory> _codecs = new();

        static CoalescedFile()
        {
            _codecs.Add(Games.Me1, (name) => new Me1.Codec(name));
            _codecs.Add(Games.Me2, (name) => new Me1.Codec(name)); // Mass Effect 2 uses the same format as ME1
            _codecs.Add(Games.Me3, (name) => new Me3.Codec(name));
        }


        public static Container Load(Games game, byte[] data, string name)
        {
            if (!_codecs.ContainsKey(game))
            {
                throw new GameNotSupportedException(game);
            }

            ICodec codec = _codecs[game](name);
            Container container = codec.Decode(data);

            return container;
        }

        public static Container Load(Games game, byte[] data)
        {
            return Load(game, data, $"New {Enum.GetName(typeof(Games), game)} Loading Codec");
        }

        public static Container Load(Games game, string fileLocation, string name)
        {
            byte[] data = File.ReadAllBytes((fileLocation));

            return Load(game, data, name);
        }

        public static Container Load(Games game, string fileLocation)
        {
            byte[] data = File.ReadAllBytes((fileLocation));

            return Load(game, data, $"New {Enum.GetName(typeof(Games), game)} Loading Codec");
        }




        public static byte[] Save(Games game, Container container, string name)
        {
            if (!_codecs.ContainsKey(game))
            {
                throw new GameNotSupportedException(game);
            }

            ICodec codec = _codecs[game](name);
            byte[] data = codec.Encode(container);

            return data;
        }

        public static byte[] Save(Games game, Container container)
        {
            return Save(game, container, $"New {Enum.GetName(typeof(Games), game)} Loading Codec");
        }

        public static void Save(Games game, Container container, string outputLocation, string name)
        {
            byte[] data = Save(game, container, name);

            File.WriteAllBytes(outputLocation, data);
        }

        public static void Compare(Games game, byte[] oldData, byte[] newData)
        {
            ICodec oCodec = _codecs[game]("old");
            ICodec nCodec = _codecs[game]("new");

            Container oContainer = oCodec.Decode(oldData);
            Container nContainer = nCodec.Decode(newData);

            oCodec.Dump();
            nCodec.Dump();

            oContainer.DumpRecords("old.container.txt");
            nContainer.DumpRecords("new.container.txt");
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


        private static void DumpStringTable(Me3.Codec codec, StreamWriter output)
        {
            foreach (var entry in codec.StringTable.Entries)
            {
                output.WriteLine("[{0,8}] ({1,10}) {2}", entry.Offset, entry.Checksum, entry.Value);
            }
        }
    }
}