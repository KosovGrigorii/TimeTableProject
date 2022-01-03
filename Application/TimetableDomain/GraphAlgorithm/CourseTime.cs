using System;

namespace TimetableDomain
{
    public class CourseTime
    {
        public Course Course { get; init; }
        public TimeSpan Time { get; init; } 
        public bool IsTransformable { get; init; }

        public CourseTime(Course course, TimeSpan time, bool isTransformable)
        {
            Course = course;
            Time = time;
            IsTransformable = isTransformable;
        }
    }
}