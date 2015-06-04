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
            SumUpSession_Case01();
            SumUpSession_Case02();

        }

        public void SumUpSession_Case01()
        {
            var sessions = new List<SessionItem>
            {
                CreateTestSession(new DateTime(2015, 6, 1, 8, 0, 0), "11111", 8),
                CreateTestSession(new DateTime(2015, 6, 2, 8, 0, 0), "11111", 6),
                CreateTestSession(new DateTime(2015, 6, 3, 8, 0, 0), "11111", 5),
                CreateTestSession(new DateTime(2015, 6, 4, 8, 0, 0), "11111", 7)
            };

            int expected = 8 + 6 + 5 + 7;
            CsvFactory factory = new CsvFactory(sessions, null);
            int result = factory.SumUpSessions(sessions);
            Assert.AreEqual(expected, result, "Case 01");
            
        }

        public void SumUpSession_Case02()
        {
            var sessions = new List<SessionItem>
            {
                CreateTestSession(new DateTime(2015, 6, 1, 8, 0, 0), "11111", 0),
                CreateTestSession(new DateTime(2015, 6, 2, 8, 0, 0), "11111", 6),
                CreateTestSession(new DateTime(2015, 6, 4, 8, 0, 0), "11111", 18),
                CreateTestSession(new DateTime(2015, 6, 8, 8, 0, 0), "11111", 2)
            };

            int expected = 0 + 6 + 18 + 2;
            CsvFactory factory = new CsvFactory(sessions, null);
            int result = factory.SumUpSessions(sessions);
            Assert.AreEqual(expected, result, "Case 02");

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
