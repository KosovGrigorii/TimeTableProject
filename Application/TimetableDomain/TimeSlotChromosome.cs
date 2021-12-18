using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    struct TimeSlotChromosome
    {
        public TimeSpan StartAt { get; set; }
        public TimeSpan EndAt => StartAt.Add(TimeSpan.FromHours(1.5));
        public string Course { get; set; }
        public string Place { get; set; }
        public string Teacher { get; set; }
        
        public List<string> Groups { get; set; }
        public int Day { get; set; }
        
        public bool IsOverlappedBy(TimeSlotChromosome other)
        {
            if (Day == other.Day)
            {
                var sameStart = StartAt == other.StartAt;
                return (sameStart || StartAt <= other.EndAt 
                                  && StartAt >= other.StartAt 
                                  || EndAt >= other.StartAt 
                                  && EndAt <= other.EndAt);
            }

            return false;
        }
    }
}