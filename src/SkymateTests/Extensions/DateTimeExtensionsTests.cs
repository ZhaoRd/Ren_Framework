using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skymate.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skymate.Extensions.Tests
{
    using System.Fakes;

    using Microsoft.QualityTools.Testing.Fakes;

    [TestClass()]
    public class DateTimeExtensionsTests
    {
        [TestMethod()]
        public void GetFirstDayOfMonthTest()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2015, 10, 21);

                var now = DateTime.Now;

                var dayOfMonth = now.GetFirstDayOfMonth();

                Assert.AreEqual(dayOfMonth.Year, 2015);
                Assert.AreEqual(dayOfMonth.Month, 10);
                Assert.AreEqual(dayOfMonth.Day, 1);
            }

        }

        [TestMethod()]
        public void GetLastDayOfMonthTest()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2015, 10, 21);

                var now = DateTime.Now;

                var dayOfMonth = now.GetLastDayOfMonth();

                Assert.AreEqual(dayOfMonth.Year, 2015);
                Assert.AreEqual(dayOfMonth.Month, 10);
                Assert.AreEqual(dayOfMonth.Day, 31);
            }
        }

        [TestMethod()]
        public void GetFirstDayOfWeekTest()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2015, 10, 21);

                var now = DateTime.Now;

                var dayOfMonth = now.GetFirstDayOfWeek();

                Assert.AreEqual(dayOfMonth.Year, 2015);
                Assert.AreEqual(dayOfMonth.Month, 10);
                Assert.AreEqual(dayOfMonth.Day, 19);
            }
        }

        [TestMethod()]
        public void GetLastDayOfWeekTest()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2015, 10, 21);

                var now = DateTime.Now;

                var dayOfMonth = now.GetLastDayOfWeek();

                Assert.AreEqual(dayOfMonth.Year, 2015);
                Assert.AreEqual(dayOfMonth.Month, 10);
                Assert.AreEqual(dayOfMonth.Day, 25);
            }
        }

        [TestMethod()]
        public void GetFirstDayOfYearTest()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2015, 10, 21);

                var now = DateTime.Now;

                var dayOfMonth = now.GetFirstDayOfYear();

                Assert.AreEqual(dayOfMonth.Year, 2015);
                Assert.AreEqual(dayOfMonth.Month, 1);
                Assert.AreEqual(dayOfMonth.Day, 1);
            }
        }

        [TestMethod()]
        public void GetLastDayOfYearTest()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2015, 10, 21);

                var now = DateTime.Now;

                var dayOfMonth = now.GetLastDayOfYear();

                Assert.AreEqual(dayOfMonth.Year, 2015);
                Assert.AreEqual(dayOfMonth.Month, 12);
                Assert.AreEqual(dayOfMonth.Day, 31);
            }
        }

        [TestMethod()]
        public void ToStartOfTheDayTest()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2015, 10, 21, 21, 19, 30);

                var now = DateTime.Now;

                var dayOfMonth = now.ToStartOfTheDay();

                Assert.AreEqual(dayOfMonth.Year, 2015);
                Assert.AreEqual(dayOfMonth.Month, 10);
                Assert.AreEqual(dayOfMonth.Day, 21);
                Assert.AreEqual(dayOfMonth.Hour, 0);
                Assert.AreEqual(dayOfMonth.Minute, 0);
                Assert.AreEqual(dayOfMonth.Second, 0);
            }
        }

        [TestMethod()]
        public void ToEndOfTheDayTest()
        {
            using (ShimsContext.Create())
            {
                ShimDateTime.NowGet = () => new DateTime(2015, 10, 21, 21, 19, 30);

                var now = DateTime.Now;

                var dayOfMonth = now.ToEndOfTheDay();

                Assert.AreEqual(dayOfMonth.Year, 2015);
                Assert.AreEqual(dayOfMonth.Month, 10);
                Assert.AreEqual(dayOfMonth.Day, 21);
                Assert.AreEqual(dayOfMonth.Hour, 23);
                Assert.AreEqual(dayOfMonth.Minute, 59);
                Assert.AreEqual(dayOfMonth.Second, 59);
            }
        }
    }
}