using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;


namespace TimeTracker.BusinessLogic
{
    /**
    * The following class contains all methods & helpers to calculate the dashboard informations
    * e.g. Vacation Days, Overtime
    */

    public class DashboardInformation
    {
        #region Global variables sessions and user data
        private ObservableCollection<SessionItem> _sessionItems;
        public ObservableCollection<SessionItem> SessionItems { get; set; }
        private readonly UserItem _user;
        #endregion
        
        //Empty Constuctor for testing puposes
        public DashboardInformation()
        {

        }

        //Default constructor: gets alls sessions and the user data from the registration
        public DashboardInformation(ObservableCollection<SessionItem> sessionItems, UserItem user)
        {
            if (sessionItems == null)
            {
                _sessionItems = new ObservableCollection<SessionItem>();
            }
            else
            {
                _sessionItems = sessionItems;
            }
            _user = user;
        }

        //Calculates overtime of the given user and sessions from the initialization
        public int CalculateOvertime()
        {
            
            return CalculateOvertime(SessionItems.ToList(), _user);
        }

        //Calculates overtime from given data
        public int CalculateOvertime(List<SessionItem> sessions, UserItem user)
        {

            int overTimeInHours = 0;

            sessions = new List<SessionItem>(sessions.OrderBy(item => item.TimestampStart));

            if (sessions.Count() != 0)
            {
                int startTime = sessions.First().TimestampStart;

                int stopTime = sessions.Last().TimestampStop;


                DateTime startDate = UnixTimeStampToDateTime(startTime);
                DateTime stopDate = UnixTimeStampToDateTime(stopTime);

                int amountOfHolidays = Holidays.HolidaysInbetween(startDate, stopDate);
                long amountOfWorkdays = CalculateWorkdays(startDate, stopDate);
                long recordedTimeInSeconds = SumUpSessions(sessions);

                double hoursPerDay = user.WorkingTime / 5.0;
                long debtInSeconds = (long)((amountOfWorkdays - amountOfHolidays) * hoursPerDay * 60 * 60);

                long overTimeInSeconds = recordedTimeInSeconds - debtInSeconds;
                overTimeInHours = (int)(overTimeInSeconds / (60 * 60));
            }
            else
            {
                overTimeInHours = 0;
            }

            return user.OverTime + overTimeInHours;

        }

        //This method converts an unix timestamp to a DateTime object
        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        
        /**
	 * This method calculates the amount of workdays that exist inbetween the given interval.
	 *
	 * @param start the start date of the interval
	 * @param stop the stop date of the interval
	 * @return the amount of workdays
	 * methodtype helper method
	 * pre start != null && stop != null
	 * post correct amount of workdays calculated
	 */
        public int CalculateWorkdays(DateTime start, DateTime stop)
        {

            //backup on which weekday the intervall started
            DayOfWeek startWeekday = start.DayOfWeek;

            //start interval on mondays
            start = start.AddDays(-DiffToMonday(startWeekday));

            //backup on which weekday the intervall stopped
            DayOfWeek stopWeekday = stop.DayOfWeek;
            //end interval on mondays
            stop = stop.AddDays(-DiffToMonday(stopWeekday));


            //calc
            int days = ((Utils.TotalDays(stop) - Utils.TotalDays(start)));
            int workDays = (int)(days * (5.0 / 7.0));
            Debug.WriteLine("Monday to mondays " + workDays);

            if (startWeekday == DayOfWeek.Sunday || startWeekday == DayOfWeek.Saturday)
            {
                startWeekday = DayOfWeek.Saturday;
            }

            if (stopWeekday == DayOfWeek.Sunday || stopWeekday == DayOfWeek.Saturday)
            {
                stopWeekday = DayOfWeek.Friday;
            }
            Debug.WriteLine("Start week delta " + DiffToMonday(startWeekday));
            Debug.WriteLine("Stop week delta " + DiffToMonday(stopWeekday));


            return workDays - DiffToMonday(startWeekday) + DiffToMonday(stopWeekday) + 1;
        }

        //Returns the offset in days to monday from a given weekday
        public int DiffToMonday(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
                default:
                    return 0;
            }
        }

        /**
	 * This method sums up the recorded time of the sessions in the list and returns the time in millis.
	 *
	 * @param sessions the list of sessions that should be summed up
	 * @return the recorded time from the sessions in seconds
	 * methodtype helper method
	 */
        protected int SumUpSessions(List<SessionItem> sessions)
        {
            return sessions.Aggregate(0, (current, session) => current + session.TotalTime);
        }

        /**
	 * This method calculates the amount of vacation days left for the user based on the recorded sessions for the
	 * default project vacation in the database.
	 *
	 * @return the amount of left vacation days
	 * @throws SQLException
	 * methodtype get method
	 */
        public int LeftVacationDays()
        {
            IEnumerable<SessionItem> items = _sessionItems.Where(a =>
                    a.ProjectId == DatabaseManager.ProjectHolidayId);


            long vacationInSeconds = SumUpSessions(items.ToList());

            double hoursPerDay = _user.WorkingTime / 5.0;
            int vacationInDays = (int)(vacationInSeconds / (60 * 60 * hoursPerDay));

            return _user.VacationDays - vacationInDays - _user.CurrentVacationDays;
        }
    }
}
