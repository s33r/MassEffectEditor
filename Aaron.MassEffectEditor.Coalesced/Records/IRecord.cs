using System.Collections;

namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public interface IRecord : IEnumerable
    {
        string Name { get; set; }
        string Path { get; }
        IRecord Parent { get; }
    }
}