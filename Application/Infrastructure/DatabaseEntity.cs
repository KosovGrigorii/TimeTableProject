namespace Infrastructure
{
    public class DatabaseEntity<TKey>
    {
        public TKey Id { get; set; }
        public TKey KeyId { get; set; }
    }
}