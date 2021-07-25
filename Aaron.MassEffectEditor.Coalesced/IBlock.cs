using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced
{
    internal interface IBlock<in T> where T: ICodec
    {
        void Read(byte[] data, T codec);
        void Write(BinaryWriter output, T codec);
        void Validate(T codec);
        void Dump(string rootName);
        string Dump();
    }
}
