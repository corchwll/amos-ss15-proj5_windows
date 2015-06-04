using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.PersonalInformation;

namespace TimeTracker
{
    public class CsvFactory
    {

        private List<SessionItem> _sessions;
        private List<ProjectItem> _projects;

        public CsvFactory(List<SessionItem> sessions, List<ProjectItem> projects)
        {
            _sessions = sessions;
            _projects = projects;
        }

        public string CreateCsvAsString()
        {
            return "";
        }

        public string CreateHeader()
        {
            return "";
        }

        public string CreateRows()
        {
            return "";
        }

        public string CreateRow(DateTime day)
        {
            return "";
        }

        public string CreateDayColumn(DateTime day)
        {
            return "";
        }

        public string CreateProjectColumns()
        {
            return "";
        }

        public string CreateProjectCell()
        {
            return "";
        }


        public int SumUpSessionsFromDateAndProject(DateTime day, string projectId)
        {
            var daySessions = QuerySessionsByDay(day);
            return SumUpSessions(daySessions.Where(item => item.ProjectId == projectId).ToList());
        }

        public List<SessionItem> QuerySessionsByDay(DateTime day)
        {
            int start = TotalSeconds(new DateTime( day.Year, day.Month, day.Day, 0,0,0));
            int end = TotalSeconds(new DateTime(day.Year, day.Month, day.Day, 23, 59, 59));

            return _sessions.Where(item => item.TimestampStart >= start && item.TimestampStart < end).ToList();
        } 

        /**
        * This method sums up the recorded time of the sessions in the list and returns the time in millis.
        *
        * @param sessions the list of sessions that should be summed up
        * @return the recorded time from the sessions in seconds
        * methodtype helper method
        */
        public int SumUpSessions(List<SessionItem> sessions)
        {
            return sessions.Aggregate(0, (current, session) => current + session.TotalTime);
        }

        private int TotalSeconds(DateTime date)
        {
            return (Int32)(date.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;


        }
    }
}
