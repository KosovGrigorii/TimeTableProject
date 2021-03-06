using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.IO;
using TimetableApplication;

namespace UserInterface
{
    [TestFixture]
    public class TestXlsxParser
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
            var times = new Times() { Duration = 90, LessonStarts = new List<TimeSpan>() };
            times.LessonStarts.Add(new TimeSpan(9, 0, 0));
            times.LessonStarts.Add(new TimeSpan(10, 40, 0));
            times.LessonStarts.Add(new TimeSpan(12, 50, 0));
            times.LessonStarts.Add(new TimeSpan(14, 30, 0));
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput1.xlsx");
            using Stream stream = new FileStream(path, FileMode.Open);
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
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput2.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(CheckResult(new UserInput() { CourseSlots = slots, TimeSchedule = times }, result));
        }

        [Test]
        public void NoTimeTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput3.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.Duration == 0 && result.TimeSchedule.LessonStarts.Count() == 0);
        }

        [Test]
        public void NoInformationTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput4.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.CourseSlots.Count() == 0);
        }

        [Test]
        public void NoSlotsTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput5.xlsx");
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
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput6.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(CheckResult(new UserInput() { CourseSlots = slots, TimeSchedule = times }, result));
        }

        [Test]
        public void NoTransferTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput7.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void NotEnoughMainArgumentsTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput8.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void NotEnoughMainArgumentsTest2()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput9.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectExtraArgumentTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput10.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectFourthMainArgumentTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput11.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectThirdMainArgumentTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput12.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectSecondMainArgumentTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput13.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void IncorrectFirstMainArgumentTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput14.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }

        [Test]
        public void SuperfluousExtraArgumentTest()
        {
            var parser = new XlsxInputParser();
            var exePath = AppDomain.CurrentDomain.BaseDirectory.Split("\\");
            exePath = exePath.Take(exePath.Length - 4).ToArray();
            var path = Path.Combine(string.Join("\\", exePath) + "\\", @"TestParsers\XlsxTest\xlsxInput15.xlsx");
            using FileStream stream = new FileStream(path, FileMode.Open);
            var result = parser.GetResult(stream);
            Assert.IsTrue(result.TimeSchedule.LessonStarts == null);
        }
    }
}
