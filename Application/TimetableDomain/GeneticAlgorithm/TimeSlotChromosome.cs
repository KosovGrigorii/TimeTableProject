using System;


namespace TimetableDomain
{
    struct TimeSlotChromosome
    {
        public TimeSpan StartAt { get; set; }
        public TimeSpan EndAt { get; init; }
        public Course Course { get; init; }
        public string Teacher { get; init; }
        public string Group { get; init; }
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