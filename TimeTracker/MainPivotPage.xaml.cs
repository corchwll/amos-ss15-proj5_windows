using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace TimeTracker
{
    public partial class MainPivotPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private DispatcherTimer dispatcherTimer;
        private Int32 totalSeconds;
        private Boolean isTimerRunning = false;

        private int currentTimestampStart;
        private int currentTimestampStop;
        private string currentProjectName;
        private string currentProjectId;

        private string ProjectHolidayName = "Holiday";
        private string ProjectHolidayId = "id_holiday";

        private string ProjectTrainingName = "Training";
        private string ProjectTrainingId = "id_training";

        private string ProjectIllnessName = "Illness";
        private string ProjectIllnessId = "id_illness";


        private LocalDataContext localDB;

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

        #region Page Lifecycle Methods



        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {

        }



        // Konstruktor
        public MainPivotPage()
        {
            InitializeComponent();
            InitTimer();

            //local
            localDB = new LocalDataContext(LocalDataContext.DBConnectionString);
            this.DataContext = this;

        }

        //OnNavigateTo is called when the page is showen as the app launches
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            bool defaultProjectsExist = false;
            using (LocalDataContext db = new LocalDataContext(LocalDataContext.DBConnectionString))
            {
                if (db.DatabaseExists() == false)
                {
                    Debug.WriteLine("Database created");
                    db.CreateDatabase();
                }
                else
                {
                    defaultProjectsExist = true;
                }
            }
            var sessionItemsInDB = from SessionItem todo in localDB.SessionItems select todo;
            var projectItemsInDB = from ProjectItem todo in localDB.ProjectItems select todo;
            var userItemsInDB = from UserItem todo in localDB.UserItems select todo;
            SessionItems = new ObservableCollection<SessionItem>(sessionItemsInDB);
            ProjectItems = new ObservableCollection<ProjectItem>(projectItemsInDB);
            UserItems = new ObservableCollection<UserItem>(userItemsInDB);

            if (!defaultProjectsExist)
            {
                CreateDefaultProjects();
            }
            base.OnNavigatedTo(e);


            string start = "";
            string end = "";
            string id = "";


            if (NavigationContext.QueryString.TryGetValue("start", out start))
            {
                NavigationContext.QueryString.TryGetValue("end", out end);

                int startConverted = Int32.Parse(start);
                int endConverted = Int32.Parse(end);
                NavigationContext.QueryString.TryGetValue("id", out id);
                createNewSessionItem(id, startConverted, endConverted);
                ShellToast toast = new ShellToast();
                toast.Title = "Saved";
                toast.Content = "Session was added";
                toast.Show();

            }

            string name = "";
            if (NavigationContext.QueryString.TryGetValue("name", out name))
            {

                string surname = "";
                NavigationContext.QueryString.TryGetValue("surname", out surname);

                string personalId = "";
                NavigationContext.QueryString.TryGetValue("personalId", out personalId);

                string workingTimeString = "";
                NavigationContext.QueryString.TryGetValue("workingTime", out workingTimeString);
                int workingTime = Int32.Parse(workingTimeString);


                string overtimeString = "";
                NavigationContext.QueryString.TryGetValue("workingTime", out overtimeString);
                int overtime = Int32.Parse(overtimeString);

                string vacationDaysString = "";
                NavigationContext.QueryString.TryGetValue("workingTime", out vacationDaysString);
                int vacationDays = Int32.Parse(vacationDaysString);

                string currentVacationString = "";
                NavigationContext.QueryString.TryGetValue("workingTime", out currentVacationString);
                int currentVacation = Int32.Parse(currentVacationString);
                createNewUserItem(name, surname, personalId, workingTime, overtime, vacationDays, currentVacation);

            }
            else if (!userExists())
            {
                NavigationService.Navigate(new Uri("/RegistrationPage.xaml", UriKind.Relative));
            }
           
        }

        #endregion

        private void CreateDefaultProjects()
        {
            createNewProjectItem(ProjectHolidayId, ProjectHolidayName);
            createNewProjectItem(ProjectTrainingId, ProjectTrainingName);
            createNewProjectItem(ProjectIllnessId, ProjectIllnessName);
            localDB.SubmitChanges();
        }

        //initialize the timer, set 1000ms as a tick interval and link the eventHandler
        private void InitTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        //EventHandler for each timer tick. Updates the textBox with the current time passed
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            totalSeconds++;
            TextBoxTime.Text = "" + getMinutes(totalSeconds) + ":" + getSeconds(totalSeconds);

        }

        //following methods build the callback functionalities of the buttons
        //implemented in the UI
        #region Clicklisteners

        private void startRecordingProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                if (!isTimerRunning)
                {
                    currentTimestampStart = getUnixTimestamp();
                    totalSeconds = 0;
                    TextBoxTime.Text = "00:00";
                    isTimerRunning = true;
                    dispatcherTimer.Start();
                    button.Content = "stop";
                    ProjectItem projectItem = button.DataContext as ProjectItem;
                    currentProjectId = projectItem.ProjectId;
                    
                    currentProjectName = projectItem.ProjectName;
                }
                //if the timer is already running, it will get stopoped, the button content will get changed and
                //the output is showen in the UI
                else
                {
                    currentTimestampStop = getUnixTimestamp();
                    isTimerRunning = false;
                    dispatcherTimer.Stop();
                    totalSeconds = currentTimestampStop - currentTimestampStart;
                    TextBoxTime.Text = "" + getMinutes(totalSeconds) + ":" + getSeconds(totalSeconds);
                    button.Content = "start";
                    createNewSessionItem(currentProjectId, currentTimestampStart, currentTimestampStop);
                    saveChangesToDatabase();
                }
            }
        }

        private void newProject_Click(object sender, RoutedEventArgs e)
        {
            createNewProjectItem(NewProjectIdTextBox.Text, NewProjectNameTextBox.Text);
        }

        private void queryData_Click(object sender, RoutedEventArgs e)
        {
            testQueryDatabase();
        }

        private void editProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ProjectItem projectItem = button.DataContext as ProjectItem;
            string projectId = projectItem.ProjectId;
            string projectName = projectItem.ProjectName;
            NavigationService.Navigate(new Uri("/EditProjectPage.xaml?id="+projectId, UriKind.Relative));
        }

        #endregion

        //following methods provide functionalities to make small calculations
        #region Utils
        //returns the amount of minutes of a total amount of seconds as a 2 digits String
        //example 65 -> "01",  620 -> "10"
        private String getMinutes(Int32 seconds){
            Int32 result = seconds/60;
            if (result < 10)
            {
                return "0" + result;
            }
            return "" + result;
        }

        //returns the rest of seconds of a total amount of seconds removing minutes as a 2 digits String
        //example: 65 -> "05", 620 -> "20"
        private String getSeconds(Int32 seconds)
        {
            Int32 result = seconds % 60;
            if(result < 10){
                return "0" + result;
            }
            return "" + result;
        }

        private Int32 getUnixTimestamp()
        {
            return (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        #endregion

        //following methods provide functionalities to bind the UI elements to the data 
        //stored in the database
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


        //following methods provide functionalities to manipulate the local database
        //as creating new session or project items and save them to the database
        #region Database interactions

        public void createNewSessionItem(string projectId, int timestampStart, int timestampStop)
        {
            SessionItem newSession = new SessionItem { ProjectId = projectId, TimestampStart = timestampStart, TimestampStop = timestampStop};
            SessionItems.Add(newSession);
            localDB.SessionItems.InsertOnSubmit(newSession);
            saveChangesToDatabase();
        }

        private void createNewProjectItem(string projectId, string projectName)
        {
            ProjectItem newProject = new ProjectItem { ProjectId = projectId, ProjectName = projectName };
            ProjectItems.Add(newProject);
            localDB.ProjectItems.InsertOnSubmit(newProject);
            saveChangesToDatabase();
        }

        private void createNewUserItem(string name, string surname, string personalId, int workingtime, int overtime, int vacationDays, int currentVacation)
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
            localDB.UserItems.InsertOnSubmit(newUser);
            saveChangesToDatabase();
        }


        private void saveChangesToDatabase()
        {
            localDB.SubmitChanges();
        }

        private void testQueryDatabase()
        {
            var sessionItemsInDB = from SessionItem todo in localDB.SessionItems select todo;
            SessionItems = new ObservableCollection<SessionItem>(sessionItemsInDB);
            foreach (var item in SessionItems)
            {
                Debug.WriteLine("Session No. " + item.SessionItemId + " on project " + item.ProjectId + " with total amount of " + getMinutes(item.TimestampStop - item.TimestampStart) + ":" + getSeconds(item.TimestampStop - item.TimestampStart));
            }
        }

        private bool userExists()
        {
            var userItemInDB = from UserItem todo in localDB.UserItems select todo;
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