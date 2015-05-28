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
