namespace Infrastructure
{
    public interface IDictionaryType<TIn, TOut>
    {
        string Name { get; }
        TOut GetImplementation(TIn parameters);
    }
}