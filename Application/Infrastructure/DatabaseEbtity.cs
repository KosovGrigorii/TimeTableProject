namespace Infrastructure
{
    public class DatabaseEbtity<TKey>
    {
        public TKey Id { get; set; }
        public TKey KeyId { get; set; }
    }
}