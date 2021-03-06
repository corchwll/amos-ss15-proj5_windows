﻿/*
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace TimeTracker
{
    public class CsvFactory
    {

        private readonly List<SessionItem> _sessions;
        private readonly List<ProjectItem> _projects;
        private readonly UserItem _user;
        private const string Separator = ";";
        private const string Newline = "\n";

        public CsvFactory(List<SessionItem> sessions, List<ProjectItem> projects, UserItem user)
        {
            if (sessions != null)
            {
                _sessions = sessions.OrderBy(item => item.TimestampStart).ToList(); 
            }
            _projects = projects;
            _user = user;
        }

        public void CreateCsvFile()
        {
            string result = CreateCsvAsString();
            WriteToFile(result);
        }

        public void DebugFile()
        {
            ReadFile();
        }

        private async Task WriteToFile(string data)
        {
            // Get the text data from the textbox. 
            byte[] fileBytes = System.Text.Encoding.UTF8.GetBytes(data.ToCharArray());

            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            // Create a new folder name DataFolder.
            var dataFolder = await local.CreateFolderAsync("DataFolder",
                CreationCollisionOption.OpenIfExists);

            // Create a new file named DataFile.txt.
            var file = await dataFolder.CreateFileAsync("DataFile.txt",
            CreationCollisionOption.ReplaceExisting);

            // Write the data from the textbox.
            using (var s = await file.OpenStreamForWriteAsync())
            {
                s.Write(fileBytes, 0, fileBytes.Length);
            }
        }

        private async Task ReadFile()
        {
            // Get the local folder.
            StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (local != null)
            {
                // Get the DataFolder folder.
                var dataFolder = await local.GetFolderAsync("DataFolder");

                // Get the file.
                var file = await dataFolder.OpenStreamForReadAsync("DataFile.txt");

                // Read the data.
                using (StreamReader streamReader = new StreamReader(file))
                {
                    Debug.WriteLine(streamReader.ReadToEnd());
                }

            }
            
        }

        public string CreateCsvAsString()
        {
            return CreateUserHeader() + CreateHeader() + CreateRows();
        }

        public string CreateUserHeader()
        {
            DateTime today = new DateTime();
            return _user.Name + Separator + _user.Surname + Separator + today.Month + "." + today.Year + Newline ;
        }

        public string CreateHeader()
        {
            string result = "Dates;";
            foreach (var item in _projects)
            {
                result += item.ProjectName;
                if (_projects.Last().Equals(item))
                {
                    result += Newline;
                }
                else
                {
                    result += Separator;
                } 
            }
            return result;
        }

        public string CreateRows()
        {
            DateTime current = Utils.GetDateTimeObject(_sessions.First().TimestampStart);
            DateTime end = Utils.GetDateTimeObject(_sessions.Last().TimestampStart);
            string result = "";
            while (Utils.TotalDays(current) <= Utils.TotalDays(end))
            {
                result += CreateRow(current);
                current = current.AddDays(1);
            }

            return result;
        }

        public string CreateRow(DateTime day)
        {
            return CreateDayCell(day) + CreateProjectCells(day);
        }

        public string CreateDayCell(DateTime day)
        {
            return day.Day.ToString() + "."
                + day.Month.ToString() + "."
                + day.Year.ToString() + Separator;
        }

        public string CreateProjectCells(DateTime day)
        {
            var result = "";
            foreach (var item in _projects)
            {
                result += CreateProjectCell(day, item.ProjectId);
                if (_projects.Last().Equals(item))
                {
                    result += Newline;
                }
                else
                {
                    result += Separator;
                }
            }

            return result;
        }

        public string CreateProjectCell(DateTime day, string projectId)
        {
            return CreateTimeString(SumUpSessionsFromDateAndProject(day, projectId));
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

        public string CreateTimeString(int seconds)
        {
            int hours = (seconds / (60 * 60));
            int remaining = seconds - (hours*60*60);
            int minutes = (remaining / 60);



            return ConvertDigitToString(hours) + ":" + ConvertDigitToString(minutes);

        }

        public string ConvertDigitToString(int digit)
        {
            if (digit < 10)
            {
                return "0" + digit.ToString();
            }
            else
            {
                return digit.ToString();
            }
            
        }
    }
}
