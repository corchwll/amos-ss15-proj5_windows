using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker.BusinessLogic;

namespace UnitTestTimeTracker
{
    [TestClass]
    public class DashboardInformationTest
    {
        [TestMethod]
        public void CalculateWorkdaysTest()
        {
            DateTime start = new DateTime(2015, 6, 1);
            DateTime stop = new DateTime(2015, 6, 8);
            int count = 6;

            DashboardInformation info = new DashboardInformation();

            int result = info.CalculateWorkdays(start, stop);

            Assert.AreEqual(count, result, "dates in 2015");

            start = new DateTime(2015, 6, 1);
            stop = new DateTime(2015, 6, 7);
            count = 5;
            result = info.CalculateWorkdays(start, stop);
            Assert.AreEqual(count, result, "dates in 2015");

            start = new DateTime(2015, 6, 1);
            stop = new DateTime(2015, 6, 12);
            count = 10;
            result = info.CalculateWorkdays(start, stop);
            Assert.AreEqual(count, result, "dates in 2015");

            start = new DateTime(2015, 5, 3);
            stop = new DateTime(2015, 6, 18);
            count = 34;
            result = info.CalculateWorkdays(start, stop);
            Assert.AreEqual(count, result, "dates in 2015");

            start = new DateTime(2015, 2, 10);
            stop = new DateTime(2015, 4 ,1);
            count = 37;
            result = info.CalculateWorkdays(start, stop);
            Assert.AreEqual(count, result, "dates in 2015");


        }
    }
}
