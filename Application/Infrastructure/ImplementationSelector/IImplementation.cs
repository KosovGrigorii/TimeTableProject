namespace Infrastructure
{
    public interface IImplementation<TIn, TOut>
    {
        string Name { get; }
        TOut GetResult(TIn parameters);
    }
}