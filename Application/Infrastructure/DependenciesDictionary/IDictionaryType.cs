namespace Infrastructure
{
    public interface IDictionaryType<TIn, TOut>
    {
        string Name { get; }
        TOut GetResult(TIn parameters);
    }
}