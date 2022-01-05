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
        private readonly IEnumerable<string> algorithms;

        public FilterInterface(IDatabaseWrapper<string, DatabaseSlot> slotWrapper, IEnumerable<ITimetableMaker> algorithms)
        {
            this.slotWrapper = slotWrapper;
            this.algorithms = algorithms.Select(a => a.Algorithm.Name);
        }
        
        public IEnumerable<string> GetAlgorithmNames()
            => algorithms;
        
        public IEnumerable<string> GetTeachers(User user)
            => slotWrapper
                .ReadBy(user.Id)
                .Select(x => x.Teacher)
                .Distinct();
    }
}