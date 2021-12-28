using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    struct TimeSlotChromosome
    {
        public TimeSpan StartAt { get; set; }
        public TimeSpan EndAt { get; set; }
        public Course Course { get; set; }
        public string Teacher { get; set; }
        public string Group { get; set; }
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