using System;

namespace TimetableDomain
{
    public class CourseTime
    {
        public Course Course { get; set; }
        public TimeSpan Time { get; set; } 
        public bool IsTransformable { get; set; }

        public CourseTime(Course course, TimeSpan time, bool isTransformable)
        {
            Course = course;
            Time = time;
            IsTransformable = isTransformable;
        }
    }
}