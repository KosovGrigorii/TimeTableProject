namespace Infrastructure
{
    public abstract class DatabaseEntity<TKey>
    {
        public TKey Id { get; init; }
        public TKey KeyId { get; init; }
    }
}