using System;
using System.Collections.Generic;
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
    }
}
