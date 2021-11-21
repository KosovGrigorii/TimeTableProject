using System;
using System.Collections.Generic;
using System.Text;
using TimetableDomain;

namespace TimetableApplication
{
    public class SortClass
    {
        public static void Sort(List<SlotInfo> tableInfo)
        {
            foreach (var slotInfo in tableInfo)
            {
                if (slotInfo.Course != null) DataBase.AddCourse(ParseSlotCourse(slotInfo.Course));
                if (slotInfo.Group != null) DataBase.AddGroup(ParseSlotGroup(slotInfo.Group));
                if (slotInfo.Class != null) DataBase.AddClass(ParseSlotClass(slotInfo.Group));
                if (slotInfo.Teacher != null) DataBase.AddTeacher(ParseSlotTeacher(slotInfo.Group));
            }
        }
        //"{\"Name\":\"Tom\",\"Age\":35}"
        //Над форматом обработки ведутся работы
        private static Course ParseSlotCourse(string _course)
        {
            return new Course();
        }

        private static Group ParseSlotGroup(string _group)
        {
            return new Group();
        }

        private static Class ParseSlotClass(string _class)
        {
            return new Class();
        }

        private static Teacher ParseSlotTeacher(string _teacher)
        {
            return new Teacher();
        }
    }
}
