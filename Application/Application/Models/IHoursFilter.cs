namespace Application
{
    public interface IHoursFilter
    {
        string FilterName { get; }
        int hours { get; }
    }
    
    //часы в день
    //часы преподавателя в день
    //...
}