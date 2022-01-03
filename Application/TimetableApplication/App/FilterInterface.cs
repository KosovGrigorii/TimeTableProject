using System;
using System.Collections.Generic;
using System.Linq;
using TimetableDomain;
using Infrastructure;

namespace TimetableApplication
{
    public class FilterInterface
    {
        private readonly IDatabaseWrapper<string, DatabaseSlot> slotWrapper;
        
        public FilterInterface(IDatabaseWrapper<string, DatabaseSlot> slotWrapper)
        {
            this.slotWrapper = slotWrapper;
        }
        
        public Array GetAlgorithmNames()
            => Enum.GetValues(typeof(Algorithm));
        
        public IEnumerable<string> GetTeachers(User user)
            => slotWrapper
                .ReadBy(user.Id)
                .Select(x => x.Teacher)
                .Distinct();
    }
}