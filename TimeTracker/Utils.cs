using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeTracker
{
    class Utils
    {

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

        public static Int32 GetUnixTimestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

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
