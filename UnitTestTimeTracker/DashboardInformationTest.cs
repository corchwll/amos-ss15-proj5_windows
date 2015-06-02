using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeTracker;
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
            stop = new DateTime(2015, 6, 1);
            count = 1;
            result = info.CalculateWorkdays(start, stop);
            Assert.AreEqual(count, result, "only one day");

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

        [TestMethod]
        public void CalculateOvertimeTest()
        {
           CalculateOvertimeTestCase01();
           CalculateOvertimeTestCase02();
           CalculateOvertimeTestCase03();

        }

        private void CalculateOvertimeTestCase01()
        {
            List<SessionItem> sessions = new List<SessionItem>();
            sessions.Add(CreateTestSessiontItem(5, 4, 8));
            sessions.Add(CreateTestSessiontItem(5, 5, 8));
            sessions.Add(CreateTestSessiontItem(5, 6, 8));

            DashboardInformation info = new DashboardInformation();
            int overtime = 0;
            int result = info.CalculateOvertime(sessions, CreateTestUserItem(0));
            Assert.AreEqual(overtime, result);
        }

        private void CalculateOvertimeTestCase02()
        {
            List<SessionItem> sessions = new List<SessionItem>();
            sessions.Add(CreateTestSessiontItem(5, 4, 8));

            DashboardInformation info = new DashboardInformation();
            int overtime = 0;
            int result = info.CalculateOvertime(sessions, CreateTestUserItem(0));
            Assert.AreEqual(overtime, result);
        }

        private void CalculateOvertimeTestCase03()
        {
            List<SessionItem> sessions = new List<SessionItem>();
            sessions.Add(CreateTestSessiontItem(5, 5, 4));
            sessions.Add(CreateTestSessiontItem(5, 6, 12));
            sessions.Add(CreateTestSessiontItem(5, 7, 5));
            sessions.Add(CreateTestSessiontItem(5, 8, 7));


            DashboardInformation info = new DashboardInformation();
            int overtime = -4;
            int result = info.CalculateOvertime(sessions, CreateTestUserItem(0));
            Assert.AreEqual(overtime, result);
        }

        private SessionItem CreateTestSessiontItem(int month, int day, int hours)
        {
            DateTime start = new DateTime(2015, month, day, 8, 0, 0);
            DateTime end = new DateTime(2015, month, day, 8 + hours, 0, 0);
            SessionItem item = new SessionItem
            {
                TimestampStart = TotalSeconds(start),
                TimestampStop = TotalSeconds(end),
                ProjectId = "12345",
                TotalTime = TotalSeconds(end) - TotalSeconds(start),
            };
            return item;

        }

        private UserItem CreateTestUserItem(int overtime)
        {
            UserItem newUser = new UserItem
            {
                Name = "Daniel",
                Surname = "Lohse",
                PersonalId = "12345",
                WorkingTime = 40,
                OverTime = overtime,
                VacationDays = 30,
                CurrentVacationDays = 0

            };
            return newUser;
        }

        private int TotalSeconds(DateTime date)
        {
            return (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;


        }
    }
}
