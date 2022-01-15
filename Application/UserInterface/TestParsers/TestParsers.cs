using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO;
using TimetableApplication;

namespace UserInterface
{

    [TestFixture]
    public class TestParsers
    {
        //private static readonly Stream stream = new FileStream("Запрос расписания1.txt", FileMode.Open);

        [Test]
        public void SingleTest()
        {
            var slots = new List<SlotInfo>();
            slots[0] = new SlotInfo() { Course = "Математика", Group = "1", Room = "100", Teacher = "Тичер" };
            slots[1] = new SlotInfo() { Course = "Физика", Group = "1", Room = "200", Teacher = "Тичер2" };
            var times = new Times() { Duration = 90, LessonStarts = new List<TimeSpan>()};
            times.LessonStarts.Append(new TimeSpan(9, 0, 0));
            times.LessonStarts.Append(new TimeSpan(10, 40, 0));
            times.LessonStarts.Append(new TimeSpan(12, 50, 0));
            times.LessonStarts.Append(new TimeSpan(14, 30, 0));
            Assert.AreEqual(new UserInput() {CourseSlots = slots, TimeSchedule = times}, new TxtInputParser().GetResult(null));
        }
    }
}
