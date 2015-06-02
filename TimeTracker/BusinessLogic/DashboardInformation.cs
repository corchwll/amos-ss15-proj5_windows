using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.BusinessLogic
{
    class DashboardInformation
    {

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
        protected long CalculateWorkdays(DateTime start, DateTime stop)
        {
            
            //backup on which weekday the intervall started
            DayOfWeek startWeekday = start.DayOfWeek;
            
            //start interval on mondays
            start.AddDays(-startWeekday.CompareTo(DayOfWeek.Monday));

            
            //backup on which weekday the intervall stopped
            DayOfWeek stopWeekday = stop.DayOfWeek;
            //end interval on mondays
            stop.AddDays(-stopWeekday.CompareTo(DayOfWeek.Monday));

            //calc
            long days = (stop.Ticks - start.Ticks) / (1000 * 60 * 60 * 24);
            long workDays = days * 5 / 7;

            if (startWeekday == DayOfWeek.Sunday)
            {
                startWeekday = DayOfWeek.Monday;
            }

            if (stopWeekday == DayOfWeek.Sunday)
            {
                stopWeekday = DayOfWeek.Monday;
            }

            return workDays - startWeekday.CompareTo(DayOfWeek.Monday) + stopWeekday.CompareTo(DayOfWeek.Monday) + 1;
        }
    }
}
