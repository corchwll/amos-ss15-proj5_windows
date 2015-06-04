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
        public void CreateRowsTest()
        {
            CreateRowsTest_Case01();
        }

        public void CreateRowsTest_Case01()
        {
            var session1 = CreateTestSession(new DateTime(2015, 6, 1, 8, 0, 0), "11111", 3);
            var session2 = CreateTestSession(new DateTime(2015, 6, 1, 13, 0, 0), "11112", 1);
            var session3 = CreateTestSession(new DateTime(2015, 6, 1, 14, 55, 0), "22222", 5);
            var session4 = CreateTestSession(new DateTime(2015, 6, 1, 19, 0, 0), "11111", 1);
            var session5 = CreateTestSession(new DateTime(2015, 6, 2, 1, 0, 0), "11111", 2);
            var session6 = CreateTestSession(new DateTime(2015, 6, 2, 8, 0, 0), "11112", 3);
            var session7 = CreateTestSession(new DateTime(2015, 6, 3, 9, 0, 0), "22222", 4);

            var sessions = new List<SessionItem>
            {
               session3, session1, session4, session6, session5, session2, session7
            };

            var project1 = CreateTestProjectItem("11111");
            var project2 = CreateTestProjectItem("11112");
            var project3 = CreateTestProjectItem("22222");

            var projects = new List<ProjectItem>
            {
                project1, project2, project3
            };

            var expected = 
                "1.6.2015," + "04:00,01:00,05:00\n" +
                "2.6.2015," + "02:00,03:00,00:00\n" +
                "3.6.2015," + "00:00,00:00,04:00\n";
            CsvFactory factory = new CsvFactory(sessions, projects);
            var result = factory.CreateRows();
            Assert.AreEqual(expected, result);
            
        }

        [TestMethod]
        public void CreateProjectCellsTest()
        {
            CreateProjectCellsTest_Case01();
        }

        public void CreateProjectCellsTest_Case01()
        {
            var session1 = CreateTestSession(new DateTime(2015, 6, 1, 8, 0, 0), "11111", 3);
            var session2 = CreateTestSession(new DateTime(2015, 6, 1, 13, 0, 0), "11112", 1);
            var session3 = CreateTestSession(new DateTime(2015, 6, 1, 14, 55, 0), "22222", 5);
            var session4 = CreateTestSession(new DateTime(2015, 6, 1, 19, 0, 0), "11111", 1);
            var session5 = CreateTestSession(new DateTime(2015, 6, 2, 1, 0, 0), "11111", 2);
            var session6 = CreateTestSession(new DateTime(2015, 6, 2, 1, 0, 0), "11111", 3);

            var sessions = new List<SessionItem>
            {
               session3, session1, session4, session6, session5, session2
            };

            var project1 = CreateTestProjectItem("11111");
            var project2 = CreateTestProjectItem("11112");
            var project3 = CreateTestProjectItem("22222");

            var projects = new List<ProjectItem>
            {
                project1, project2, project3
            };

            var expected = "04:00,01:00,05:00\n";
            CsvFactory factory = new CsvFactory(sessions, projects);
            var result = factory.CreateProjectCells(new DateTime(2015, 6, 1));
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void SumUpSessionsFromDateAndProjectTest()
        {
            SumUpSessionsFromDateAndProjectTest_Case01();
            SumUpSessionsFromDateAndProjectTest_Case02();
        }

        public void SumUpSessionsFromDateAndProjectTest_Case01()
        {
            var session1 = CreateTestSession(new DateTime(2015, 6, 1, 8, 0, 0), "11111", 3);
            var session2 = CreateTestSession(new DateTime(2015, 6, 1, 13, 0, 0), "11112", 1);
            var session3 = CreateTestSession(new DateTime(2015, 6, 1, 14, 55, 0), "22222", 5);
            var session4 = CreateTestSession(new DateTime(2015, 6, 1, 19, 0, 0), "11111", 1);
            var session5 = CreateTestSession(new DateTime(2015, 6, 2, 1, 0, 0), "11111", 2);
            var session6 = CreateTestSession(new DateTime(2015, 6, 2, 1, 0, 0), "11111", 3);

            var sessions = new List<SessionItem>
            {
               session3, session1, session4, session6, session5, session2
            };

            var expected = 4 * 60 * 60;
            CsvFactory factory = new CsvFactory(sessions, null);
            var result = factory.SumUpSessionsFromDateAndProject(new DateTime(2015, 6, 1), "11111");
            Assert.AreEqual(expected, result);

        }

        public void SumUpSessionsFromDateAndProjectTest_Case02()
        {
            var session1 = CreateTestSession(new DateTime(2015, 6, 3, 8, 0, 0), "11111", 3);
            var session2 = CreateTestSession(new DateTime(2015, 6, 1, 13, 0, 0), "11112", 1);
            var session3 = CreateTestSession(new DateTime(2015, 6, 1, 14, 55, 0), "22222", 5);
            var session4 = CreateTestSession(new DateTime(2015, 6, 3, 19, 0, 0), "11111", 1);
            var session5 = CreateTestSession(new DateTime(2015, 6, 2, 1, 0, 0), "11111", 2);
            var session6 = CreateTestSession(new DateTime(2015, 6, 2, 1, 0, 0), "11111", 3);

            var sessions = new List<SessionItem>
            {
               session3, session1, session4, session6, session5, session2
            };

            var expected = 0;
            CsvFactory factory = new CsvFactory(sessions, null);
            var result = factory.SumUpSessionsFromDateAndProject(new DateTime(2015, 6, 1), "11111");
            Assert.AreEqual(expected, result);

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

            int expected = (8 + 6 + 5 + 7) * 60 * 60;
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

            int expected = (0 + 6 + 18 + 2) * 60 * 60;
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
                TotalTime = totalHours * 60 * 60,
                ProjectId = projectId

            };

            return item;
        }

        public ProjectItem CreateTestProjectItem(string projectId)
        {
            ProjectItem item = new ProjectItem
            {
                ProjectId = projectId,
                ProjectName = "random"
            };
            return item;
        }

        private int TotalSeconds(DateTime date)
        {
            return (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;


        }
    }
}
