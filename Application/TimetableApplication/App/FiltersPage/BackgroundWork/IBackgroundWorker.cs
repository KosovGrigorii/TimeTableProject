namespace TimetableApplication
{
    public interface IBackgroundWorker
    {
        IBackgroundTaskQueue TaskQueue { get; }
    }
}