using System.Collections.Generic;

namespace TimetableApplication
{
    public record ParticularTimetable(Dictionary<string, string[,]> Table);
}