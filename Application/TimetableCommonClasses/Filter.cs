namespace TimetableCommonClasses
{
    public class Filter
    {
        public string Category { get; }
        public string Name { get; }
        public int Days { get; }

        public Filter(string category, string name, int daysCount)
        {
            Category = category;
            Name = name;
            Days = daysCount;
        }
    }
}