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
        public void QuerySessionsByDayTest()
        {
            QuerySessionsByDayTest_Case01();
            QuerySessionsByDayTest_Case02();
        }

        public void QuerySessionsByDayTest_Case01()
        {
            var session1 = CreateTestSession(new DateTime(2015, 6, 1, 0, 0, 0), "11111", 8);
            var session2 =CreateTestSession(new DateTime(2015, 6, 1, 8, 0, 0), "11111", 19);
            var session3 = CreateTestSession(new DateTime(2015, 6, 1, 14, 55, 0), "11111", 5);
            var session4 =CreateTestSession(new DateTime(2015, 6, 2, 0, 0, 0), "11111", 7);
            var session5 = CreateTestSession(new DateTime(2015, 6, 2, 1, 0, 0), "11111", 7);

            var sessions = new List<SessionItem>
            {
               session1, session2, session3, session4, session5
            };

            CsvFactory factory = new CsvFactory(sessions, null);
            var result = factory.QuerySessionsByDay(new DateTime(2015,6,1,0,0,0));
            Assert.IsTrue(result.Contains(session1), "Session 01 should be contained");
            Assert.IsTrue(result.Contains(session2), "Session 02 should be contained");
            Assert.IsTrue(result.Contains(session3), "Session 03 should be contained");
            Assert.IsFalse(result.Contains(session4), "Session 01 should not be contained");
            Assert.IsFalse(result.Contains(session5), "Session 05 should not be contained");

        }

        public void QuerySessionsByDayTest_Case02()
        {
            var session1 = CreateTestSession(new DateTime(2015, 6, 1, 0, 0, 0), "11111", 8);
            var session2 = CreateTestSession(new DateTime(2015, 6, 1, 8, 0, 0), "11111", 19);
            var session3 = CreateTestSession(new DateTime(2015, 6, 1, 14, 55, 0), "11111", 5);
            var session4 = CreateTestSession(new DateTime(2015, 6, 2, 0, 0, 0), "11111", 7);
            var session5 = CreateTestSession(new DateTime(2015, 6, 2, 1, 0, 0), "11111", 7);

            var sessions = new List<SessionItem>
            {
               session3, session5, session2, session4, session1
            };

            CsvFactory factory = new CsvFactory(sessions, null);
            var result = factory.QuerySessionsByDay(new DateTime(2015, 6, 1, 0, 0, 0));
            Assert.IsTrue(result.Contains(session1), "Session 01 should be contained");
            Assert.IsTrue(result.Contains(session2), "Session 02 should be contained");
            Assert.IsTrue(result.Contains(session3), "Session 03 should be contained");
            Assert.IsFalse(result.Contains(session4), "Session 01 should not be contained");
            Assert.IsFalse(result.Contains(session5), "Session 05 should not be contained");

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
