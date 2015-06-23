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

using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;

namespace TimeTracker
{
    [Table]
    public class SessionItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _sessionItemId;

        public SessionItem(string id)
        {
            ProjectId = id;
        }

        public SessionItem()
        {
            
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int SessionItemId
        {
            get
            {
                return _sessionItemId;
            }
            set
            {
                if (_sessionItemId != value)
                {
                    NotifyPropertyChanging("SessionItemId");
                    _sessionItemId = value;
                    NotifyPropertyChanged("SessionItemId");
                }
            }
        }


        private int _timestampStart;

        [Column]
        public int TimestampStart
        {
            get
            {
                return _timestampStart;
            }
            set
            {
                if (_timestampStart != value)
                {
                    NotifyPropertyChanging("TimestampStart");
                    _timestampStart = value;
                    NotifyPropertyChanged("TimestampStart");
                }
            }
        }

        private int _timestampStop;
        [Column]
        public int TimestampStop
        {
            get
            {
                return _timestampStop;
            }
            set
            {
                if (_timestampStop != value)
                {
                    NotifyPropertyChanging("TimestampStop");
                    _timestampStop = value;
                    NotifyPropertyChanged("TimestampStop");
                    TotalTime = TimestampStop - TimestampStart;
                    int hours = TotalTime/ (60*60);
                    int minutes = (TotalTime/60) - (hours*60);
                    DateTime dateStart = UnixTimeStampToDateTime(TimestampStart);
                    DateTime dateEnd = UnixTimeStampToDateTime(TimestampStop);
                    VisualText = dateStart.Day.ToString() + "." +
                                 dateStart.Month.ToString() + "  " +
                                 dateStart.Hour.ToString() + ":" + 
                                 dateStart.Minute.ToString() + " - " +
                                 dateEnd.Hour.ToString() + ":" +
                                 dateEnd.Minute.ToString() + "  " +
                                 ConvertDigitToString(hours) + ":" +
                                 ConvertDigitToString(minutes);



                }
            }
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

        private string _projectId;

        [Column]
        public string ProjectId
        {
            get
            {
                return _projectId;
            }
            set
            {
                if (_projectId != value)
                {
                    NotifyPropertyChanging("ProjectId");
                    _projectId = value;
                    NotifyPropertyChanged("ProjectId");

                }
            }
        }

        private int _totalTime;
        [Column]
        public int TotalTime
        {
            get
            {
                return _totalTime;
            }
            set
            {
                if (_totalTime != value)
                {
                    NotifyPropertyChanging("TotalTime");
                    _totalTime = value;
                    NotifyPropertyChanged("TotalTime");
                }
            }
        }


        [Column]
        public double Longitude { get; set; }

        [Column]
        public double Latitude { get; set; }


        public void AddPosition(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }
        private string _visualText;
        public string VisualText { get; set; }

        public static DateTime UnixTimeStampToDateTime(int unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
