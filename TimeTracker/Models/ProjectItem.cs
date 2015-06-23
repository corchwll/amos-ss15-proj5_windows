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

using System.Data.Linq.Mapping;
using System.ComponentModel;

namespace TimeTracker
{
    //Following class defines the table structure for project items
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

        private bool _archived;

        [Column]
        public bool Archived
        {
            get
            {
                return _archived;
            }
            set
            {
                if (_archived != value)
                {
                    NotifyPropertyChanging("Archived");
                    _archived = value;
                    NotifyPropertyChanged("Archived");
                }
            }
        }

        [Column]
        public double Latitude { get; set; }

        [Column]
        public double Longitude { get; set; }

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
        //Check if the project ID is valid. (5 digits, only numbers)
        public static bool CheckProjectId(string projectId)
        {
            int idNumber;
            if (int.TryParse(projectId, out idNumber))
            {
                if (projectId.Length != 5)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
