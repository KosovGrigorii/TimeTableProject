namespace TimetableApplication
{
    public class Filter
    {
        public string Name { get; }
        public int Days { get; }

        public Filter(string name, int daysCount)
        {
            Name = name;
            Days = daysCount;
        }
    }
}