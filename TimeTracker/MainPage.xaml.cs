using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Phone.Controls;



using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Diagnostics;

namespace TimeTracker
{

    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private DispatcherTimer dispatcherTimer;
        private Int32 totalSeconds;
        private Boolean isTimerRunning = false;

        private int currentTimestampStart;
        private int currentTimestampStop;
        private string currentProjectName;
        private string currentProjectId;


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

        #region Page Lifecycle Methods

        // Konstruktor
        public MainPage()
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
            var sessionItemsInDB = from SessionItem todo in localDB.SessionItems select todo;
            var projectItemsInDB = from ProjectItem todo in localDB.ProjectItems select todo;

            SessionItems = new ObservableCollection<SessionItem>(sessionItemsInDB);
            ProjectItems = new ObservableCollection<ProjectItem>(projectItemsInDB);
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

            }
           
        }

        #endregion


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
            textBoxTime.Text = "" + getMinutes(totalSeconds) + ":" + getSeconds(totalSeconds);

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
                    textBoxTime.Text = "00:00";
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
                    textBoxTime.Text = "" + getMinutes(totalSeconds) + ":" + getSeconds(totalSeconds);
                    button.Content = "start";
                    createNewSessionItem(currentProjectId, currentTimestampStart, currentTimestampStop);
                    saveChangesToDatabase();
                }
            }
        }

        private void newProject_Click(object sender, RoutedEventArgs e)
        {
            createNewProjectItem(newProjectIdTextBox.Text, newProjectNameTextBox.Text);
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
    }



    
}