using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public interface IRecord: IEnumerable
    {
        string Name { get; set; }
        string Path { get; }
        IRecord Parent { get; }

        
    }
}
