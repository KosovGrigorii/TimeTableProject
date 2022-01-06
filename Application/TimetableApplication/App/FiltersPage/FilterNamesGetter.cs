using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;

namespace TimetableApplication
{
    public class FilterNamesGetter
    {
        private readonly IDatabaseWrapper<string, DatabaseSlot> slotWrapper;

        public FilterNamesGetter(IDatabaseWrapper<string, DatabaseSlot> slotWrapper)
        {
            this.slotWrapper = slotWrapper;
        }
        
        public IEnumerable<string> GetTeachers(User user)
            => slotWrapper
                .ReadBy(user.Id)
                .Select(x => x.Teacher)
                .Distinct();
    }
}