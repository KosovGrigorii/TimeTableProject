using System;
using System.Collections.Generic;

namespace TimetableDomain
{
    struct TimeSlotChromosome
    {
        public TimeSpan StartAt { get; set; }
        public TimeSpan EndAt => StartAt.Add(TimeSpan.FromHours(3));
        public string CourseId { get; set; }
        public string PlaceId { get; set; }
        public string TeacherId { get; set; }
        
        public int Group { get; set; }
        public int Day { get; set; }
    }
}