using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public int SumUpSessionsFromDateAndProject(DateTime date, string projectId)
        {
            var daySessions = new List<SessionItem>();

            foreach (var item in daySessions.Where(item => item.ProjectId == projectId))
            {
                daySessions.Add(item);
            }
            return SumUpSessions(daySessions);
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
    }
}
