using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TimetableDomain
{
    public interface ITimetableMaker
    {
        List<TimeSlot> Start(List<Course> cources, List<string> classes);
    }
}