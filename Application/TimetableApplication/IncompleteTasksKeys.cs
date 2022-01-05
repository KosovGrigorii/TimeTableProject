using System.Collections.Generic;

namespace TimetableApplication
{
    public class IncompleteTasksKeys
    {
        public HashSet<string> UserIds { get; }

        public IncompleteTasksKeys()
        {
            UserIds = new HashSet<string>();
        }
    }
}