using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.BusinessLogic
{
    public class DashboardInformation
    {
        private ObservableCollection<SessionItem> _sessionItems;
        public ObservableCollection<SessionItem> SessionItems { get; set; }

        private readonly UserItem _user;

        public DashboardInformation()
        {
            
        }

        public DashboardInformation(ObservableCollection<SessionItem> sessionItems, UserItem user)
        {
            _sessionItems = sessionItems;
            _user = user;
        }


        public int CalculateOvertime(List<SessionItem> sessions, UserItem user)
	    {

		int overTimeInHours = 0;

		if(sessions.Count() != 0)
		{
			int startTime = sessions.First().TimestampStart;
									 
			int stopTime = sessions.Last().TimestampStop;
									

			//int amountOfHolidays = Holidays.getHolidaysInbetween(startTime, stopTime);
			//long amountOfWorkdays = calculateWorkdays(startTime, stopTime);
			//long recordedTimeInMillis = sumUpSessions(sessions);

			//double hoursPerDay = currentUser.getWeeklyWorkingTime()/5.0;
			//long debtInMillis = (long)((amountOfWorkdays - amountOfHolidays)*hoursPerDay*60*60*1000);

			//long overTimeInMillis = recordedTimeInMillis - debtInMillis;
			//overTimeInHours = (int)(overTimeInMillis/(1000*60*60));
		} else
		{
			overTimeInHours = 0;
		}

		return user.OverTime + overTimeInHours;
	    
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
            int days = (( TotalDays(stop) - TotalDays(start)));
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

        public int TotalDays(DateTime date)
        {
            return (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalDays;

            
        }
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
            return sessions.Aggregate(0,(current, session) => current + session.TotalTime);
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
        IEnumerable<SessionItem> items = SessionItems.Where(a =>
                a.ProjectId == DatabaseManager.ProjectHolidayId);


		long vacationInSeconds = SumUpSessions(items.ToList());

		double hoursPerDay = _user.WorkingTime/5.0;
		int vacationInDays = (int)(vacationInSeconds/(60*60*hoursPerDay));

		return _user.VacationDays - vacationInDays - _user.CurrentVacationDays;
	}
    }
}
