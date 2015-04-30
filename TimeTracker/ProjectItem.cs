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
    public class ProjectItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int _projectItemId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ProjectItemId
        {
            get
            {
                return _projectItemId;
            }
            set
            {
                if (_projectItemId != value)
                {
                    NotifyPropertyChanging("ProjectItemId");
                    _projectItemId = value;
                    NotifyPropertyChanged("ProjectItemId");
                }
            }
        }

        private string _projectId;

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


        private string _projectName;

        [Column]
        public string ProjectName
        {
            get
            {
                return _projectName;
            }
            set
            {
                if (_projectName != value)
                {
                    NotifyPropertyChanging("ProjectName");
                    _projectName = value;
                    NotifyPropertyChanged("ProjectName");
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
