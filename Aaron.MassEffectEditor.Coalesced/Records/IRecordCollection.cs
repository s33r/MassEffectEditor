namespace Aaron.MassEffectEditor.Coalesced.Records
{
    public interface IRecordCollection<out T>
    {
        T Values { get; }
    }
}