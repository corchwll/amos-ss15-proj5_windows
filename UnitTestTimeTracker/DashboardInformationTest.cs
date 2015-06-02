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
            DateTime start = new DateTime(2015, 5, 3);
            DateTime stop = new DateTime(2015, 6, 18);
            long count = 35;

            DashboardInformation info = new DashboardInformation();

            long result = info.CalculateWorkdays(start, stop);

            Assert.AreEqual(count, result, "dates in 2015");


        }
    }
}
