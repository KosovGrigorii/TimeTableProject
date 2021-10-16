namespace Application
{
    public interface IInputParser
    {
        void ReadFile();
        void FillHoursDB(params IHoursFilter[] filters);
    }
}