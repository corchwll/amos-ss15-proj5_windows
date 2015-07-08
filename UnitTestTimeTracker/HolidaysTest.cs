using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker.BusinessLogic;

namespace UnitTestTimeTracker
{
    [TestClass]
    public class HolidaysTest
    {

        [TestMethod]
        public void IsGivenDayAHolidayTest()
        {
            int year1 = 2015;

            DateTime date1 = new DateTime(year1, 1, 1);
            DateTime date2 = new DateTime(year1, 1, 6);
            DateTime date3 = new DateTime(year1, 5, 1);
            DateTime date7 = new DateTime(year1, 12, 25);

            DateTime date4 = new DateTime(year1, 8, 15);
            DateTime date5 = new DateTime(year1, 10, 3);
            DateTime date6 = new DateTime(year1, 11, 1);
            DateTime date8 = new DateTime(year1, 12, 26);

            Assert.IsTrue(Holidays.IsGivenDateAHoliday(date1));
            Assert.IsTrue(Holidays.IsGivenDateAHoliday(date2));
            Assert.IsTrue(Holidays.IsGivenDateAHoliday(date3));
            Assert.IsTrue(Holidays.IsGivenDateAHoliday(date7));

            //Assert.IsFalse(Holidays.IsGivenDateAHoliday(date4));
            //Assert.IsFalse(Holidays.IsGivenDateAHoliday(date5));
            //Assert.IsFalse(Holidays.IsGivenDateAHoliday(date6));
            //Assert.IsFalse(Holidays.IsGivenDateAHoliday(date8));

        }



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

        [TestMethod]
        public void AmountOfHolidaysInBetweenTest()
        {

            int year = 2015;
            DateTime startDate = new DateTime(year, 2, 5);
            DateTime endDate = new DateTime(year, 10, 3);
            int count = 1;
            List<DateTime> holidays = Holidays.FixedHolidays(year);
            int resultCount = Holidays.AmountOfHolidaysBetween(holidays, startDate, endDate);

            Assert.AreEqual(count, resultCount);

        }

        [TestMethod]
        public void AmountOfHolidaysSinceTest()
        {
            int year = 2015;
            List<DateTime> holidays = Holidays.FixedHolidays(year);
            DateTime date = new DateTime(year, 1,7);
            int count = 2;
            int result = Holidays.AmountOfHolidaysSince(holidays, date);
            Assert.AreEqual(count, result);

            year = 2015;
            holidays = Holidays.FixedHolidays(year);
            date = new DateTime(year, 1, 1);
            count = 3;
            result = Holidays.AmountOfHolidaysSince(holidays, date);
            Assert.AreEqual(count, result);
        }

        [TestMethod]
        public void AmountOfHolidaysUntil()
        {
            int year = 2015;
            List<DateTime> holidays = Holidays.FixedHolidays(year);
            DateTime date = new DateTime(year, 1, 7);
            int count = 2;
            int result = Holidays.AmountOfHolidaysUntil(holidays, date);
            Assert.AreEqual(count, result);

            year = 2015;
            holidays = Holidays.FixedHolidays(year);
            date = new DateTime(year, 1, 1);
            count = 0;
            result = Holidays.AmountOfHolidaysUntil(holidays, date);
            Assert.AreEqual(count, result);   
        }

        [TestMethod]
        public void HolidaysForYear()
        {
            int year = 2015;
            List<DateTime> holidays = Holidays.HolidaysForYear(year);
            DateTime karfreitag = new DateTime(year, 4, 3);
            DateTime ostermontag = new DateTime(year, 4, 6);
            DateTime christihimmelfahrt = new DateTime(year, 5, 14);
            DateTime pfingstmontag = new DateTime(year, 5, 25);
            DateTime fronleichnam = new DateTime(year, 6, 4);


            Assert.IsTrue(holidays.Contains(karfreitag), "Karfreitag");
            Assert.IsTrue(holidays.Contains(ostermontag), "ostermontag");
            Assert.IsTrue(holidays.Contains(christihimmelfahrt), "christihimmelfahrt");
            Assert.IsTrue(holidays.Contains(pfingstmontag), "pfingstmontag");
            Assert.IsTrue(holidays.Contains(fronleichnam), "fronleichnam");


            DateTime date1 = new DateTime(year, 1, 1);
            DateTime date2 = new DateTime(year, 1, 6);
            DateTime date3 = new DateTime(year, 5, 1);
            DateTime date7 = new DateTime(year, 12, 25);

            DateTime date4 = new DateTime(year, 8, 15);
            DateTime date5 = new DateTime(year, 10, 3);
            DateTime date6 = new DateTime(year, 11, 1);
            DateTime date8 = new DateTime(year, 12, 26);

            Assert.IsTrue(holidays.Contains(date1), "New Year");
            Assert.IsTrue(holidays.Contains(date2), "Heilige 3 Koenige");
            Assert.IsTrue(holidays.Contains(date3), "Tag der Arbeit");
            Assert.IsTrue(holidays.Contains(date7), "1. Weihnachtsfeiertag");

            Assert.IsFalse(holidays.Contains(date4), "Maria Himmelfahrt");
            Assert.IsFalse(holidays.Contains(date5), "Tag der deutschen Einheit");
            Assert.IsFalse(holidays.Contains(date6), "Allerheiligen");
            Assert.IsFalse(holidays.Contains(date8), "2. Weihnachtsfeiertag");
        }

        [TestMethod]
        public void HolidaysInbetweenTest()
        {
            DateTime start = new DateTime(2015, 1,2);
            DateTime stop = new DateTime(2015, 8, 16);
            int count = 7;
            int result = Holidays.HolidaysInbetween(start, stop);
            Assert.AreEqual(count, result, "2 dates in 2015");

            start = new DateTime(2014, 8, 20);
            stop = new DateTime(2015, 8,16);
            count = 11;
            result = Holidays.HolidaysInbetween(start, stop);
            Assert.AreEqual(count, result, "2014 & 2015");
        }
    }
}
