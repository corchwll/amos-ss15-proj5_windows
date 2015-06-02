using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker.BusinessLogic
{
    public class Holidays
    {


        /**
	 * This method is used to get the amound of holidays in a given time intervall. This method only counts the
	 * holidays that are on a weekday.
	 *
	 * @param start the lower bound of the time intervall
	 * @param stop the upper bound of the time intervall
	 * @return the amount of holidays on weekdays in the given time intervall as int
	 * methodtype get method
	 * pre start before stop
	 * post correct amount will be calculated
	 */
        public static int HolidaysInbetween(DateTime start, DateTime stop)
        {

            int amountOfHolidays = 0;
            if (start.Year == stop.Year)
            {
                List<DateTime> holidays = HolidaysForYear(start.Year);
                amountOfHolidays += AmountOfHolidaysBetween(holidays, start, stop);
            }
            else if (stop.Year - start.Year == 1)
            {
                List<DateTime> holidaysInStartYear = HolidaysForYear(start.Year);
                List<DateTime> holidaysInStopYear = HolidaysForYear(stop.Year);

                amountOfHolidays += AmountOfHolidaysSince(holidaysInStartYear, start);
                amountOfHolidays += AmountOfHolidaysUntil(holidaysInStopYear, stop);
            }
            else if (stop.Year - start.Year > 1)
            {
                List<DateTime> holidaysInStartYear = HolidaysForYear(start.Year);
                List<DateTime> holidaysInStopYear = HolidaysForYear(stop.Year);

                amountOfHolidays += AmountOfHolidaysSince(holidaysInStartYear, start);
                amountOfHolidays += AmountOfHolidaysUntil(holidaysInStopYear, stop);

                for (int i = start.Year + 1; i < stop.Year; i++)
                {
                   amountOfHolidays += HolidaysForYear(i).Count();
                }
            }

            return amountOfHolidays;
        }
        /**
	 * This method is used to get all holidays on weekdays for a given year.
	 *
	 * @param year the year for which the holidays should be calculated
	 * @return a List containing all holidays on weekdays for the requested year
	 * methodtype get method
	 */
        public static List<DateTime> HolidaysForYear(int year)
        {
            List<DateTime> holidays = FixedHolidays(year);
            DateTime easterSunday = Easter(year);

            //Karfreitag
            holidays.Add(easterSunday.AddDays(-2.0));
;
            //Ostermontag
            holidays.Add(easterSunday.AddDays(1.0));

            //Christi Himmelfahrt
            holidays.Add(easterSunday.AddDays(39.0));

            //Pfingstmontag
            holidays.Add(easterSunday.AddDays(50.0));

            //Fronleichnam
            holidays.Add(easterSunday.AddDays(60.0));

            return holidays;
        }

        /**
	 * This method returns all holidays that are not depending on easter and are on a weekday for the requested year.
	 *
	 * @param year the year for which the fixed holidays should be calculated
	 * @return a list containing all fixed holidays on weekdays for this year
	 * methodtype get method
	 */
        public static List<DateTime> FixedHolidays(int year)
	    {

            List<DateTime> holidays = new List<DateTime>();

            AddIfNotWeekend(new DateTime(year, 1, 1), holidays); //New Year
            AddIfNotWeekend(new DateTime(year, 1, 6), holidays); //Heilige 3 Koenige
            AddIfNotWeekend(new DateTime(year, 5, 1), holidays); //Tag der Arbeit
            AddIfNotWeekend(new DateTime(year, 8, 15), holidays); //Maria Himmelfahrt
            AddIfNotWeekend(new DateTime(year, 10, 3), holidays); //Tag der deutschen Einheit
            AddIfNotWeekend(new DateTime(year, 11, 1), holidays); //Allerheiligen
            AddIfNotWeekend(new DateTime(year, 12, 25), holidays); //1. Weihnachtstag
            AddIfNotWeekend(new DateTime(year, 12, 26), holidays); //2. Weihnachtstag
            return holidays;

	    }

        private static bool IsWeekendDay(DateTime date)
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
            {
                return true;
            }
            return false;
        }

        private static void AddIfNotWeekend(DateTime date, List<DateTime> holidays)
        {
            if (IsWeekendDay(date))
            {
                holidays.Add(date);
            }
        }


        /**
	 * This method calculates the date for easter sunday based on the gaussian easterformula.
	 *
	 * @param year the year for which easter sunday should be calculated
	 * @return the date of easter sunday
	 * methodtype get method
	 * pre year > 1970 due to the easter formula
	 * post date of easter sunday will be calculated correctly
	 */
        public static DateTime Easter(int year)
        {
            //Osterformel
            int a = year % 19;
            int b = year % 4;
            int c = year % 7;
            int k = year / 100;
            int p = (8 * k + 13) / 25;
            int q = k / 4;
            int M = (15 + k - p - q) % 30;
            int d = (19 * a + M) % 30;
            int N = (4 + k - q) % 7;
            int e = (2 * b + 4 * c + 6 * d + N) % 7;
            int o = 22 + d + e;

            int month = 3;
            if (o > 31)
            {
                month++;
            }

            int day = o % 31;
            
            DateTime cal = new DateTime(year, month, day);
            return cal;
        }

        /**
	 * This method checks how many of the holidays will be after the start date and before the stop date.
	 *
	 * @param holidays the holidays that should be checked
	 * @param startCal the date after which the holidays have to be
	 * @param stopCal the date before which the holidays have to be
	 * @return the amount of holidays inbetween the given dates
	 * methodtype helper method
	 * pre startCal != null && stopCal != null
	 * post correct amount will be returned
	 */
        public static int AmountOfHolidaysBetween(List<DateTime> holidays, DateTime startCal, DateTime stopCal)
        {
            return holidays.Count(cal => startCal.Ticks <= cal.Ticks && stopCal.Ticks >=     cal.Ticks);
        }

        /**
	 * This method checks how many of the holidays will be after the given date.
	 *
	 * @param holidays the holidays that should be checked
	 * @param startCal the date after which the holidays have to be
	 * @return the amount of holidays after the given date
	 * methodtype helper method
	 * pre startCal != null
	 * post correct amount will be returned
	 */
        public static int AmountOfHolidaysSince(List<DateTime> holidays, DateTime startCal)
        {
            return holidays.Count(cal => startCal.Ticks < cal.Ticks);
        }

        /**
	 * This method checks how many of the holidays will be before the given date.
	 *
	 * @param holidays the holidays that should be checked
	 * @param stopCal the date before which the holidays have to be
	 * @return the amount of holidays before the given date
	 * methodtype helper method
	 * pre stopCal != null
	 * post correct amount will be returned
	 */
        public static int AmountOfHolidaysUntil(List<DateTime> holidays, DateTime stopCal)
        {
            return holidays.Count(cal => stopCal.Ticks > cal.Ticks);
        }
    }
}
