using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.Linq;

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

        public int GetTotalSeconds()
        {
            return TimestampStop - TimestampStart;
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
                }
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
