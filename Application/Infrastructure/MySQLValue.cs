namespace Infrastructure
{
    public class MySQLValue<TKey>
    {
        public TKey Id { get; set; }
        public TKey KeyId { get; set; }
    }
}