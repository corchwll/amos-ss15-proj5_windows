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
           CalculateOvertimeTestCase04();
           CalculateOvertimeTestCase05();

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
            Assert.AreEqual(overtime, result, "case 01");
        }

        private void CalculateOvertimeTestCase02()
        {
            List<SessionItem> sessions = new List<SessionItem> {CreateTestSessiontItem(5, 4, 8)};

            DashboardInformation info = new DashboardInformation();
            int overtime = 0;
            int result = info.CalculateOvertime(sessions, CreateTestUserItem(0));
            Assert.AreEqual(overtime, result, "case 02");
        }

        private void CalculateOvertimeTestCase03()
        {
            List<SessionItem> sessions = new List<SessionItem>
            {
                CreateTestSessiontItem(5, 5, 4),
                CreateTestSessiontItem(5, 6, 12),
                CreateTestSessiontItem(5, 7, 5),
                CreateTestSessiontItem(5, 8, 7)
            };


            DashboardInformation info = new DashboardInformation();
            int overtime = -2;
            int result = info.CalculateOvertime(sessions, CreateTestUserItem(2));
            Assert.AreEqual(overtime, result, "case 03");
        }

        private void CalculateOvertimeTestCase04()
        {
            List<SessionItem> sessions = new List<SessionItem>
            {
                CreateTestSessiontItem(5, 5, 4), // Tuesday 4h (-4)
                CreateTestSessiontItem(4, 30, 8), // Thursday week before 8h (0)
                //1.5.2015 tag der arbeit
                CreateTestSessiontItem(5, 4, 8), //Monday 8h
                CreateTestHolidaySessiontItem(5,6,8), //Wednesday Holiday
                CreateTestSessiontItem(5, 7, 5), //Thursday 5h (-3)
                CreateTestSessiontItem(5, 8, 7), //Friday 7h (-1)
                 //Monday 0h (-8)

                CreateTestSessiontItem(5, 12, 1) //Tuesday 1h (-7)
            };

            DashboardInformation info = new DashboardInformation();
            int overtime = -13;
            int result = info.CalculateOvertime(sessions, CreateTestUserItem(10));
            Assert.AreEqual(overtime, result,  "case 04");
        }

        private void CalculateOvertimeTestCase05()
        {
            List<SessionItem> sessions = new List<SessionItem>
            {
                CreateTestHolidaySessiontItem(4,30,8), // Thursday Holiday
                                 //Friday Tag der Arbeit
                CreateTestSessiontItem(5, 4, 10), //Monday 10h (+2)
                CreateTestSessiontItem(5, 5, 9), // Tuesday 9h (+1)
                CreateTestHolidaySessiontItem(5,6,4), //Wednesday Half Day Holiday
                CreateTestSessiontItem(5,6,4), //Wednesday 4h
                CreateTestSessiontItem(5, 7, 8), //Thursday 8h (0)
                CreateTestSessiontItem(5, 8, 7), //Friday 7h (-1)
                 //Monday 0h (-8)
                CreateTestSessiontItem(5, 12, 12) //Tuesday 12h (+4)
            };

            DashboardInformation info = new DashboardInformation();
            int overtime = 8;
            int result = info.CalculateOvertime(sessions, CreateTestUserItem(10));
            Assert.AreEqual(overtime, result, "case 05");
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

        private SessionItem CreateTestHolidaySessiontItem(int month, int day, int hours)
        {
            DateTime start = new DateTime(2015, month, day, 8, 0, 0);
            DateTime end = new DateTime(2015, month, day, 8 + hours, 0, 0);
            SessionItem item = new SessionItem
            {
                TimestampStart = TotalSeconds(start),
                TimestampStop = TotalSeconds(end),
                ProjectId = DatabaseManager.ProjectHolidayId,
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
