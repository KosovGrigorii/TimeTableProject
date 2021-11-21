using System;
using System.Collections.Generic;
using System.Text;
using TimetableDomain;

namespace TimetableApplication
{
    public static class DataBase
    {
        private static List<Course> courses = new List<Course>();
        private static List<Group> groups = new List<Group>();
        private static List<Class> classes = new List<Class>();
        private static List<Teacher> teachers = new List<Teacher>();

        public static void AddCourse(Course _course)
        {
            courses.Add(_course);
        }

        public static void AddGroup(Group _group)
        {
            groups.Add(_group);
        }

        public static void AddClass(Class _class)
        {
            classes.Add(_class);
        }

        public static void AddTeacher(Teacher _teacher)
        {
            teachers.Add(_teacher);
        }
    }
}
