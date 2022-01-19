using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;
using TimetableApplication;

namespace UserInterface
{

    [TestFixture]
    public class TestTxtParser
    {
        private bool CheckResult(UserInput expected, UserInput result)
        {
            if (!expected.TimeSchedule.Duration.Equals(result.TimeSchedule.Duration)) return false;
            if (!expected.TimeSchedule.LessonStarts.SequenceEqual(result.TimeSchedule.LessonStarts)) return false;
            if (!expected.CourseSlots.Count().Equals(result.CourseSlots.Count())) return false;
            for (var i = 0; i < expected.CourseSlots.Count(); i++)
            {
                var expSlot = expected.CourseSlots.First();
                var resSlot = result.CourseSlots.First();
                if (expSlot == null && resSlot == null) break;
                if (expSlot.Course != resSlot.Course || expSlot.Group != resSlot.Group || 
                    expSlot.Room != resSlot.Room || expSlot.Teacher != resSlot.Teacher) return false;
                expected.CourseSlots.Skip(1);
                result.CourseSlots.Skip(1);
            }
            return true;
        }

        [Test]
        public void FreeTimeTest()
        {
            var slots = new List<SlotInfo>();
            slots.Add(new SlotInfo() { Course = "Математика", Group = "1", Room = "100", Teacher = "Тичер" });
            slots.Add(new SlotInfo() { Course = "Физика", Group = "1", Room = "200", Teacher = "Тичер2" });
            var times = new Times() { Duration = 90, LessonStarts = new List<TimeSpan>()};
            times.LessonStarts.Add(new TimeSpan(9, 0, 0));
            times.LessonStarts.Add(new TimeSpan(10, 40, 0));
            times.LessonStarts.Add(new TimeSpan(12, 50, 0));
            times.LessonStarts.Add(new TimeSpan(14, 30, 0));
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput1.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(CheckResult(new UserInput() { CourseSlots = slots, TimeSchedule = times }, result));
        }

        [Test]
        public void WithoutFreeTimeTest()
        {
            var slots = new List<SlotInfo>();
            slots.Add(new SlotInfo() { Course = "Математика", Group = "1", Room = "100", Teacher = "Тичер" });
            slots.Add(new SlotInfo() { Course = "Физика", Group = "1", Room = "200", Teacher = "Тичер2" });
            var times = new Times() { Duration = 90, LessonStarts = new List<TimeSpan>() };
            times.LessonStarts.Add(new TimeSpan(9, 0, 0));
            times.LessonStarts.Add(new TimeSpan(10, 40, 0));
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput2.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(CheckResult(new UserInput() { CourseSlots = slots, TimeSchedule = times }, result));
        }

        [Test]
        public void NoTimeTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput3.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.Duration == 0 && result.TimeSchedule.LessonStarts.Count() == 0);
        }

        [Test]
        public void NoInformationTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput4.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.CourseSlots.Count() == 0);
        }

        [Test]
        public void NoSlotsTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput5.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.CourseSlots.Count() == 0);
        }

        [Test]
        public void UnfullCollomnsTest()
        {
            var slots = new List<SlotInfo>();
            slots.Add(null);
            var times = new Times() { LessonStarts = new List<TimeSpan>() };
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput6.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(CheckResult(new UserInput() { CourseSlots = slots, TimeSchedule = times }, result));
        }

        [Test]
        public void NoTransferTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput7.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void NotEnoughMainArgumentsTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput8.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void NotEnoughMainArgumentsTest2()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput9.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectExtraArgumentTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput10.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectFourthMainArgumentTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput11.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectThirdMainArgumentTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput12.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectSecondMainArgumentTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput13.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectFirstMainArgumentTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput14.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void SuperfluousExtraArgumentTest()
        {
            var parser = new TxtInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\TxtTest\txtInput15.txt");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }
    }
}
