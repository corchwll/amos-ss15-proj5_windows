using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace TimeTracker
{
    class Utils
    {

        public static string FormatSecondsToChronometerString(int seconds)
        {
            return "00:" + Utils.GetMinutes(seconds) + ":" + Utils.GetSeconds(seconds);
            
        }
        //following methods provide functionalities to make small calculations
        #region Utils
        //returns the amount of minutes of a total amount of seconds as a 2 digits String
        //example 65 -> "01",  620 -> "10"
        public static String GetMinutes(Int32 seconds){
            Int32 result = seconds/60;
            if (result < 10)
            {
                return "0" + result;
            }
            return "" + result;
        }

        //returns the rest of seconds of a total amount of seconds removing minutes as a 2 digits String
        //example: 65 -> "05", 620 -> "20"
        public static String GetSeconds(Int32 seconds)
        {
            Int32 result = seconds % 60;
            if(result < 10){
                return "0" + result;
            }
            return "" + result;
        }

        //Returns the unix timestamp in seconds
        public static Int32 GetUnixTimestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static DateTime GetDateTimeObject(int seconds)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(seconds).ToLocalTime();
            return dtDateTime;
        }

        public static int TotalDays(DateTime date)
        {
            return (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalDays;


        }


        //Returns a reverse instance of a collection with types of SessionItems
        public static ObservableCollection<SessionItem> ReverseCurrentSessionItems(ObservableCollection<SessionItem> list)
        {
            ObservableCollection<SessionItem> newList = new ObservableCollection<SessionItem>();
            foreach (var data in list.Reverse())
            {
                newList.Add(data);
            }
            return newList;
        } 

        #endregion
    }
}
