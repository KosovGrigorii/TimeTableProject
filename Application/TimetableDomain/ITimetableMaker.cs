using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TimetableDomain
{
    public interface ITimetableMaker
    {
        string Name { get; }
        List<TimeSlot> Start(List<Course> cources, List<string> classes);
    }
}