using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;


namespace TimeTracker
{
    //following methods provide functionalities to manipulate the local database
    //as creating new session or project items and save them to the database
    class DatabaseManager
    {
        private const string ProjectHolidayName = "Holiday";
        private const string ProjectHolidayId = "id_holiday";

        private const string ProjectTrainingName = "Training";
        private const string ProjectTrainingId = "id_training";

        private const string ProjectIllnessName = "Illness";
        private const string ProjectIllnessId = "id_illness";

        private readonly LocalDataContext _localDb;



        private ObservableCollection<ProjectItem> _projectItems;
        public ObservableCollection<ProjectItem> ProjectItems
        {
            get
            {
                return _projectItems;
            }
            set
            {
                if (_projectItems != value)
                {
                    _projectItems = value;
                    NotifyPropertyChanged("ProjectItems");
                }
            }
        }

        private ObservableCollection<SessionItem> _sessionItems;
        public ObservableCollection<SessionItem> SessionItems
        {
            get
            {
                return _sessionItems;
            }
            set
            {
                if (_sessionItems != value)
                {
                    _sessionItems = value;
                    NotifyPropertyChanged("SessionItems");
                }
            }
        }

        private ObservableCollection<UserItem> _userItems;
        public ObservableCollection<UserItem> UserItems
        {
            get
            {
                return _userItems;
            }
            set
            {
                if (_userItems != value)
                {
                    _userItems = value;
                }
            }
        }

        public DatabaseManager()
        {
            _localDb = new LocalDataContext(LocalDataContext.DBConnectionString);

            

            var defaultProjectsExist = CreateDatabase();

            LoadLocalData();

            if (!defaultProjectsExist)
            {
                CreateDefaultProjects();
            }

        }

        public bool CreateDatabase()
        {
            using (LocalDataContext db = new LocalDataContext(LocalDataContext.DBConnectionString))
            {
                if (db.DatabaseExists() == false)
                {
                    Debug.WriteLine("Database created");
                    db.CreateDatabase();
                    return false;
                }
                return true;
            }
        }


        public void LoadLocalData()
        {
            var sessionItemsInDb = from SessionItem todo in _localDb.SessionItems select todo;
            var projectItemsInDb = from ProjectItem todo in _localDb.ProjectItems select todo;
            var userItemsInDb = from UserItem todo in _localDb.UserItems select todo;
            SessionItems = new ObservableCollection<SessionItem>(sessionItemsInDb);
            ProjectItems = new ObservableCollection<ProjectItem>(projectItemsInDb);
            UserItems = new ObservableCollection<UserItem>(userItemsInDb);
        }

        public void CreateDefaultProjects()
        {
            createNewProjectItem(ProjectHolidayId, ProjectHolidayName);
            createNewProjectItem(ProjectTrainingId, ProjectTrainingName);
            createNewProjectItem(ProjectIllnessId, ProjectIllnessName);
            _localDb.SubmitChanges();
        }

        public void UpdateUser(UserItem newUser)
        {
            UserItem user = UserItems[0];

            _localDb.UserItems.DeleteOnSubmit(user);
            saveChangesToDatabase();
            UserItems.Clear();
            UserItems.Add(user);
            _localDb.UserItems.InsertOnSubmit(newUser);

            saveChangesToDatabase();

        }
        public void CreateNewSessionItem(string projectId, int timestampStart, int timestampStop)
        {
            SessionItem newSession = new SessionItem { ProjectId = projectId, TimestampStart = timestampStart, TimestampStop = timestampStop};
            SessionItems.Add(newSession);
            _localDb.SessionItems.InsertOnSubmit(newSession);
            saveChangesToDatabase();
        }

        public void createNewProjectItem(string projectId, string projectName)
        {
            ProjectItem newProject = new ProjectItem { ProjectId = projectId, ProjectName = projectName };
            ProjectItems.Add(newProject);
            _localDb.ProjectItems.InsertOnSubmit(newProject);
            saveChangesToDatabase();
        }

        public void createNewUserItem(string name, string surname, string personalId, int workingtime, int overtime, int vacationDays, int currentVacation)
        {
            UserItem newUser = new UserItem {
                Name = name,
                Surname = surname,
                PersonalId = personalId,
                WorkingTime = workingtime,
                OverTime = overtime,
                VacationDays = vacationDays,
                CurrentVacationDays = currentVacation
            
            };

            UserItems.Add(newUser);
            _localDb.UserItems.InsertOnSubmit(newUser);
            saveChangesToDatabase();
        }


        public void saveChangesToDatabase()
        {
            _localDb.SubmitChanges();
        }

        public void testQueryDatabase()
        {
            var sessionItemsInDB = from SessionItem todo in _localDb.SessionItems select todo;
            SessionItems = new ObservableCollection<SessionItem>(sessionItemsInDB);
            foreach (var item in SessionItems)
            {
                Debug.WriteLine("Session No. " + item.SessionItemId + " on project " + item.ProjectId + " with total amount of " 
                    + Utils.GetMinutes(item.TimestampStop - item.TimestampStart) + ":" + Utils.GetSeconds(item.TimestampStop - item.TimestampStart));
            }
        }

        public bool userExists()
        {
            var userItemInDB = from UserItem todo in _localDb.UserItems select todo;
            UserItems = new ObservableCollection<UserItem>(userItemInDB);
            int i = 0;
            foreach (var item in UserItems)
            {
                Debug.WriteLine("User name" + item.Name);
            
                i++;
            }

            if(i == 0)
            {
                return false;
            }

            return true;
        }

                   //following methods provide functionalities to bind the UI elements to the data 
        //stored in the database
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }





    //The instance of this class creates a connection to the local database
    //and provides the relevant table properties to manioukate the database
    public class LocalDataContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:/Session.sdf";


        public LocalDataContext(string connectionString) : base(connectionString)
        { }

        public Table<SessionItem> SessionItems;

        public Table<ProjectItem> ProjectItems;

        public Table<UserItem> UserItems;
    }
}
