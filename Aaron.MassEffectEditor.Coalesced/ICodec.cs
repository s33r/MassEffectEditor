using Aaron.MassEffectEditor.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced
{
    public interface ICodec
    {
        Games Game { get; }
        byte[] Encode(Container value);

        Container Decode(byte[] value);

    }
}
