/*
 *     Mobile Time Accounting
 *     Copyright (C) 2015
 *
 *     This program is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU Affero General Public License as
 *     published by the Free Software Foundation, either version 3 of the
 *     License, or (at your option) any later version.
 *
 *     This program is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU Affero General Public License for more details.
 *
 *     You should have received a copy of the GNU Affero General Public License
 *     along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace TimeTracker
{
    public class UriFactory
    {

        private string NavigationBody = "/Pages/MainPivotPage.xaml?";
        private const string LinkElement = "&";

        public string CreateProjectDataUri(string projectName, string projectId,
            string date, string latitude, string longitude)
        {
            return NavigationBody
                   + ProjectNameUri(projectName)
                   + LinkElement
                   + ProjectIdUri(projectId)
                   + LinkElement
                   + ProjectDateUri(date)
                   + LinkElement
                   + ProjectLatitude(latitude)
                   + LinkElement
                   + ProjectLongitude(longitude);
        }


        public string CreateSessionDataUri(string timestampStart, string timestampEnd, string projectId )
        {
            return NavigationBody
                   + TimestampStart(timestampStart)
                   + LinkElement
                   + TimestampEnd(timestampEnd)
                   + LinkElement
                   + ProjectIdSession(projectId);
        }

        private string TimestampStart(string start)
        {
            return QueryDictionary.SessionStart +"=" + start;
        }

        private string TimestampEnd(string end)
        {
            return QueryDictionary.SessionStop + "=" + end;
        }

        private string ProjectIdSession(string id)
        {
            return QueryDictionary.SessionProjectId + "=" + id;
        }

        


    

        public void SetNavigationBody(string body)
        {
            NavigationBody = body;
        }

        #region Project Data URI Helpers

        private string ProjectLatitude(string latitude)
        {
            return QueryDictionary.ProjectLatitude + "=" + latitude;
        }

        private string ProjectLongitude(string longitude)
        {
            return QueryDictionary.ProjectLongitude + "=" + longitude;

        }
        private string ProjectNameUri(string projectName)
        {
            return QueryDictionary.ProjectName + "=" + projectName;
        }

        private string ProjectIdUri(string projectId)
        {
            return QueryDictionary.ProjectId + "=" + projectId;

        }

        private string ProjectDateUri(string date)
        {
            return QueryDictionary.ProjectFinalDate + "=" + date;

        }

        #endregion



    }


        
    
}
