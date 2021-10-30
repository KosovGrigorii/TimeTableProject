namespace TimetableApplication
{
    public interface IOutputFormatter
    {
        void ReadTimeslots();
        byte[] GetOutputFile();
    }
}