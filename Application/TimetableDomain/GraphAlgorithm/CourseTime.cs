using System;

namespace TimetableDomain
{
    public class CourseTime
    {
        public Course Course { get; }
        public TimeSpan Time { get; } 
        public bool IsTransformable { get; }

        public CourseTime(Course course, TimeSpan time, bool isTransformable)
        {
            Course = course;
            Time = time;
            IsTransformable = isTransformable;
        }
    }
}