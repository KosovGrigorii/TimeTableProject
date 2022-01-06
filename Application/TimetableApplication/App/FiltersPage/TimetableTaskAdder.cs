using System.Collections.Generic;
using System.Threading.Tasks;

namespace TimetableApplication
{
    public class TimetableTaskAdder
    {
        private readonly IBackgroundTaskQueue taskQueue;
        private readonly TimetableLauncher launcher;

        public TimetableTaskAdder(IBackgroundTaskQueue taskQueue, TimetableLauncher launcher)
        {
            this.taskQueue = taskQueue;
            this.launcher = launcher;
        }
        
        public void AddTaskFor(User user, string algorithm, IEnumerable<Filter> applicationFilters)
        {
            taskQueue.QueueBackgroundWorkItemAsync(
                async  (token) =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        var task = new Task(() => launcher.MakeTimetable(user, algorithm, applicationFilters));
                        task.Start();
                        await task;
                    }
                });
        }
    }
}