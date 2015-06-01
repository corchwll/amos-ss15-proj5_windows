using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker.BusinessLogic;

namespace UnitTestTimeTracker
{
    [TestClass]
    public class HolidaysTest
    {
        [TestMethod]
        public void FixedHolidaysTest()
        {
            int year1 = 2015;
            int count1 = 4;
            List<DateTime> holidays = Holidays.FixedHolidays(year1);
            Assert.AreEqual(holidays.Count, count1);

            DateTime date1 = new DateTime(year1, 1, 1);
            DateTime date2 = new DateTime(year1, 1, 6);
            DateTime date3 = new DateTime(year1, 5, 1);
            
            DateTime date7 = new DateTime(year1, 12, 25);

            DateTime date4 = new DateTime(year1, 8, 15);
            DateTime date5 = new DateTime(year1, 10, 3);
            DateTime date6 = new DateTime(year1, 11, 1);
            DateTime date8 = new DateTime(year1, 12, 26);

            Assert.IsTrue(holidays.Contains(date1));
            Assert.IsTrue(holidays.Contains(date2));
            Assert.IsTrue(holidays.Contains(date3));
            Assert.IsTrue(holidays.Contains(date7));

            Assert.IsFalse(holidays.Contains(date4));
            Assert.IsFalse(holidays.Contains(date5));
            Assert.IsFalse(holidays.Contains(date6));
            Assert.IsFalse(holidays.Contains(date8));

            int year2 = 2014;
            int count2 = 7;
            holidays = Holidays.FixedHolidays(year2);
            Assert.AreEqual(holidays.Count, count2);
        }

        [TestMethod]
        public void EasterTest()
        {
            int year = 2015;
            DateTime realEaster = new DateTime(year, 4, 5);
            DateTime easter = Holidays.Easter(year);
            
            Assert.AreEqual(realEaster.Month, easter.Month);
            Assert.AreEqual(realEaster.Day, easter.Day);


        }
    }
}
