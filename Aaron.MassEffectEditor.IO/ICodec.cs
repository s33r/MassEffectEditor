using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.IO
{
    public interface ICodec<X, Y>
    {
        X Encode(Y value);

        Y Decode(X value);
        
    }
}
