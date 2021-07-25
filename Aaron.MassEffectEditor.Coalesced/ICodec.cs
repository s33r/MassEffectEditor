using Aaron.MassEffectEditor.Core;

namespace Aaron.MassEffectEditor.Coalesced
{
    public interface ICodec
    {
        Games Game { get; }
        byte[] Encode(Container value);

        Container Decode(byte[] value);
    }
}