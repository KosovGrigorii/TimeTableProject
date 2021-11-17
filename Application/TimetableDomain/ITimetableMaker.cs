using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TimetableDomain
{
    public interface ITimetableMaker
    {
        List<TimeSlot> Start(DbSet<Course> cources, DbSet<Class> classes);
    }
}