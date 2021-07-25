using System.IO;

namespace Aaron.MassEffectEditor.Coalesced
{
    internal interface IBlock<in T> where T : ICodec
    {
        void Read(byte[] data, T codec);
        void Write(BinaryWriter output, T codec);
        void Validate(T codec);
        void Dump(string rootName);
        string Dump();
    }
}