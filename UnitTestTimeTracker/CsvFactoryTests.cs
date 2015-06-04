using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker;

namespace UnitTestTimeTracker
{
    [TestClass]
    public class CsvFactoryTests
    {
        [TestMethod]
        public void SumUpSessionsFromDateAndProjectTest()
        {
        }

        [TestMethod]
        public void SumUpSessions()
        {
            List<SessionItem> sessions = new List<SessionItem>();
            sessions.Add(CreateTestSession(new DateTime(2015, 6, 1, 8, 0, 0), "11111", 8));
            sessions.Add(CreateTestSession(new DateTime(2015, 6, 1, 8, 0, 0), "11111", 8));


            CsvFactory factory = new CsvFactory();

        }

        public SessionItem CreateTestSession(DateTime day, string projectId, int totalHours)
        {
            SessionItem item = new SessionItem
            {
                TimestampStart = TotalSeconds(day),
                TimestampStop = TotalSeconds(day.AddHours(totalHours)),
                TotalTime = totalHours,
                ProjectId = projectId

            };

            return item;
        }

        private int TotalSeconds(DateTime date)
        {
            return (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;


        }
    }
}
