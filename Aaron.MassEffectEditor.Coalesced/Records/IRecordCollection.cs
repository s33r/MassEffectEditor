using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public interface IRecordCollection<out T>
    {
        T Values { get; }
    }
}
