using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;


namespace TimeTracker
{
    //following methods provide functionalities to manipulate the local database
    //as creating new session or project items and save them to the database
    public class DatabaseManager
    {
        static string ProjectHolidayName = "Holiday";
        public static string ProjectHolidayId = "00001";

        public const string ProjectTrainingName = "Training";
        public const string ProjectTrainingId = "00002";

        public const string ProjectIllnessName = "Illness";
        public const string ProjectIllnessId = "00003";

        public const string ProjectOfficeName = "Office";
        public const string ProjectOfficeId = "00004";

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

        private ObservableCollection<SessionItem> _currentSessionItems;
        public ObservableCollection<SessionItem> CurrentSessionItems
        {
            get
            {
                return _currentSessionItems;
            }
            set
            {
                if (_currentSessionItems != value)
                {
                    _currentSessionItems = value;
                    NotifyPropertyChanged("SessionItems");
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

        public bool DatabaseExists()
        {
            using (LocalDataContext db = new LocalDataContext(LocalDataContext.DBConnectionString))
            {
                return db.DatabaseExists() == false;
                
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
            var currentSessionItemsInDb = from item in _localDb.SessionItems where item.ProjectId == "00000" select item;
            CurrentSessionItems = new ObservableCollection<SessionItem>(currentSessionItemsInDb);
        }

        public void LoadCurrentSessions(string id)
        {
            var currentSessionItemsInDb = from item in _localDb.SessionItems where item.ProjectId == id select item;
            CurrentSessionItems = new ObservableCollection<SessionItem>(currentSessionItemsInDb);
            Debug.WriteLine("SessionItems found: " + CurrentSessionItems.Count);
        }

        public void CreateDefaultProjects()
        {
            createNewProjectItem(ProjectHolidayId, ProjectHolidayName);
            createNewProjectItem(ProjectTrainingId, ProjectTrainingName);
            createNewProjectItem(ProjectOfficeId, ProjectOfficeName);
            createNewProjectItem(ProjectIllnessId, ProjectIllnessName);
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
            SessionItem newSession = new SessionItem { ProjectId =projectId, TimestampStart = timestampStart, TimestampStop = timestampStop, TotalTime = timestampStop - timestampStart};
            SessionItems.Add(newSession);
            _localDb.SessionItems.InsertOnSubmit(newSession);
            saveChangesToDatabase();
        }

        public bool CreateNewSessionItem(SessionItem item)
        {
            SessionItem newSession = item;
            if (!isTimeframeFree(newSession))
            {
                return false;
            }
            SessionItems.Add(newSession);
           
            _localDb.SessionItems.InsertOnSubmit(newSession);
            saveChangesToDatabase();
            return true;
        }

        public ProjectItem createNewProjectItem(string projectId, string projectName)
        {
            ProjectItem newProject = new ProjectItem { ProjectId = projectId, ProjectName = projectName };
            ProjectItems.Add(newProject);
            _localDb.ProjectItems.InsertOnSubmit(newProject);
            saveChangesToDatabase();
            return newProject;
        }

        private bool isTimeframeFree(SessionItem newSession)
        {
            foreach (var item in _localDb.SessionItems)
            {
                if(newSession.TimestampStart >= item.TimestampStart && newSession.TimestampStart < item.TimestampStop){
                    return false;
                }

                if(newSession.TimestampStop >= item.TimestampStart && newSession.TimestampStop < item.TimestampStop){
                    return false;
                }
            }

            return true;

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

        public void createNewUserItem(UserItem newUser)
        {
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

        public void DeleteProject(ProjectItem toDoForDelete)
        {

            // Remove the to-do item from the observable collection.
            ProjectItems.Remove(toDoForDelete);

            // Remove the to-do item from the local database.
            _localDb.ProjectItems.DeleteOnSubmit(toDoForDelete);

            // Save changes to the database.
            _localDb.SubmitChanges();
        }

        public void DeleteSession(SessionItem toDoForDelete)
        {
            // Remove the to-do item from the observable collection.
            SessionItems.Remove(toDoForDelete);

            // Remove the to-do item from the local database.
            _localDb.SessionItems.DeleteOnSubmit(toDoForDelete);

            // Save changes to the database.
            _localDb.SubmitChanges();
            
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
