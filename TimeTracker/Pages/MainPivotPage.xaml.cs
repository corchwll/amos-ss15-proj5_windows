using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace TimeTracker
{
    public partial class MainPivotPage : PhoneApplicationPage
    {
        private SessionItem _currentSessionItem;

        //Timer instance for recording sessions and update UI
        private DispatcherTimer _dispatcherTimer;
        
        //Changed to True/False when timer is started/stopped
        private Boolean _isTimerRunning = false;



        //Database instance - Create, Delete, Update or Remove elements
        private readonly DatabaseManager _dataBaseManager;

        //Collection of all (default & custome) Projects
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
                    _dataBaseManager.NotifyPropertyChanged("ProjectItems");
                }
            }
        }

        //Collection of all sessions from current selected project
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
                    _dataBaseManager.NotifyPropertyChanged("SessionItems");
                }
            }
        }

        //Collection of all session items recorded on this device
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
                    _dataBaseManager.NotifyPropertyChanged("SessionItems");
                }
            }
        }

        #region Page Lifecycle Methods

        //Lifecycle method when a certain pivot item is loaded
        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Konstruktor
        public MainPivotPage()
        {
            InitializeComponent();
            InitTimer();
            this.DataContext = this;
            _dataBaseManager = new DatabaseManager();
            ProjectItems = _dataBaseManager.ProjectItems;
            CurrentSessionItems = _dataBaseManager.CurrentSessionItems;
            SessionItems = _dataBaseManager.SessionItems;
        }

        //OnNavigateTo is called when the page is showen as the app launches
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
            FillPersonalData();
            CollectNewProject();
            CollectNewSession();
            CollectRegistrationData();
        }

        #endregion

        //Catches necessary data and creates new project items in database
        //when create project interactions was executed
        private void CollectNewProject()
        {
            string id = "";
            string name = "";
            string finalDate = "";

            if (NavigationContext.QueryString.TryGetValue("projectId", out id))
            {
                NavigationContext.QueryString.TryGetValue("projectName", out name);
                NavigationContext.QueryString.TryGetValue("finalDate", out finalDate);
                
                _dataBaseManager.createNewProjectItem(id, name);
                PivotMain.SelectedIndex = 2;

            }

        }

        //Catches necessary data and creates new session items in database
        //when edit project was executed
        private void CollectNewSession()
        {
            string start = "";
            string end = "";
            string id = "";

            if (NavigationContext.QueryString.TryGetValue("start", out start))
            {
                NavigationContext.QueryString.TryGetValue("end", out end);

                int startConverted = Int32.Parse(start);
                int endConverted = Int32.Parse(end);
                NavigationContext.QueryString.TryGetValue("id", out id);
                _dataBaseManager.CreateNewSessionItem(id, startConverted, endConverted);
                ShellToast toast = new ShellToast();
                toast.Title = "Saved";
                toast.Content = "Session was added";
                toast.Show();
            }
        }

        public class QueryDictionary
        {
            public const string UserNameKey = "name";
            public const string UserSurnameKey = "surname";
            public const string UserPersonalIdKey = "personalId";
            public const string UserWorkingTimeKey = "workingTime";
            public const string UserOverTimeKey = "overtime";
            public const string UserVacationDaysKey = "vacationDays";
            public const string UserCurrentVacationDaysKey = "currentVacation";
        }

        //Catches necessary data and creates new user item in database
        //when registration was executed
        private void CollectRegistrationData()
        {
            string name = "";
            if (NavigationContext.QueryString.TryGetValue("name", out name))
            {
                UserItem newUser = new UserItem
                {
                    Name = CollectStringOnNavigation(QueryDictionary.UserNameKey),
                    Surname = CollectStringOnNavigation(QueryDictionary.UserSurnameKey),
                    PersonalId = CollectStringOnNavigation(QueryDictionary.UserPersonalIdKey),
                    WorkingTime = CollectIntOnNavgation(QueryDictionary.UserWorkingTimeKey),
                    OverTime = CollectIntOnNavgation(QueryDictionary.UserOverTimeKey),
                    VacationDays = CollectIntOnNavgation(QueryDictionary.UserVacationDaysKey),
                    CurrentVacationDays = CollectIntOnNavgation(QueryDictionary.UserCurrentVacationDaysKey)

                };
                _dataBaseManager.createNewUserItem(newUser);
                FillPersonalData(newUser);

            }
            else if (!_dataBaseManager.userExists())
            {
                NavigationService.Navigate(new Uri("/Pages/RegistrationPage.xaml", UriKind.Relative));
            }
            
        }

        private string CollectStringOnNavigation(string key)
        {
            string result = "";
            NavigationContext.QueryString.TryGetValue(key, out result);
            return result;
        }

        private int CollectIntOnNavgation(string key)
        {
            string result = "";
            NavigationContext.QueryString.TryGetValue(key, out result);
            return Int32.Parse(result);

        }

        //initialize the timer, set 1000ms as a tick interval and link the eventHandler
        private void InitTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        //EventHandler for each timer tick. Updates the textBox with the current time passed
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            int TmpEndingTime = Utils.GetUnixTimestamp();
            int TotalSeconds = TmpEndingTime - _currentSessionItem.TimestampStart;
            TextBlockCurrentTimer.Text = Utils.FormatSecondsToChronometerString(TotalSeconds);

        }

        //following methods build the callback functionalities of the buttons
        //implemented in the UI

        #region Clicklisteners

        //Click listener when user wants to start recording a new session for a project.
        //Saves the unix timestamp from the start and ending point and creates a new
        //session in the database.
        private void startRecordingProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                if (!_isTimerRunning)
                {
                    _currentSessionItem.TimestampStart = Utils.GetUnixTimestamp();
                    TextBlockCurrentTimer.Text = "00:00:00";
                    _isTimerRunning = true;
                    _dispatcherTimer.Start();
                    ButtonStartStopRecording.Content = "Stop Recording";

                }
                //if the timer is already running, it will get stopoped, the button content will get changed and
                //the output is showen in the UI
                else
                {
                    _currentSessionItem.TimestampStop = Utils.GetUnixTimestamp();
                    _isTimerRunning = false;
                    _dispatcherTimer.Stop();
                    int TotalSeconds = _currentSessionItem.TimestampStop - _currentSessionItem.TimestampStart;
                    TextBlockCurrentTimer.Text = Utils.FormatSecondsToChronometerString(TotalSeconds);
                    ButtonStartStopRecording.Content = "Start Recording";
                    _dataBaseManager.CreateNewSessionItem(_currentSessionItem);
                    _dataBaseManager.saveChangesToDatabase();
                }
            }
        }

        //Click listener when user wants to create a new project
        //App navigates to the create project page
        private void newProject_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CreateProjectPage.xaml", UriKind.Relative));
        }

        //Click listener when user wants to edit a project (add new sessions)
        //App navigates to the edit project page
        private void editProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ProjectItem projectItem = button.DataContext as ProjectItem;
            string projectId = projectItem.ProjectId;
            string projectName = projectItem.ProjectName;
            NavigationService.Navigate(new Uri("/Pages/EditProjectPage.xaml?id=" + projectId, UriKind.Relative));
        }

        //Click listener when change settings is executed.
        //User item is updated in database
        private void SavePersonalData_Click(object sender, RoutedEventArgs e)
        {
            UserItem newUser = new UserItem
            {
                Name = TextBoxName.Text,
                Surname = TextBoxSurname.Text,
                PersonalId = TextBoxPersonalId.Text,
                WorkingTime = Int32.Parse(TextBoxHoursWeek.Text),
                OverTime = Int32.Parse(TextBoxOvertime.Text),
                VacationDays = Int32.Parse(TextBoxVacation.Text),
                CurrentVacationDays = Int32.Parse(TextBoxCurrentVacation.Text)

            };
            _dataBaseManager.UpdateUser(newUser);
            FillPersonalData(newUser);
        }

        //Deletes a project after the user has acknowledged the task
        private void deleteProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            MessageBoxResult result = MessageBox.Show("Are you sure?",
                      "Deleting project", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            
            
            _dataBaseManager.DeleteProject(button);
        }

        #endregion

        //Loads the user data from the database and passes it to a fill method
        private void FillPersonalData()
        {
            if (_dataBaseManager.UserItems.Count == 0)
            {
                return;
            }
            UserItem user = _dataBaseManager.UserItems[0];

            FillPersonalData(user);
        }

        //Fills the single Ui elements with the appropriate user data
        private void FillPersonalData(UserItem user)
        {
            TextBoxName.Text = user.Name;
            TextBoxSurname.Text = user.Surname;
            TextBoxPersonalId.Text = user.PersonalId;
            TextBoxHoursWeek.Text = user.WorkingTime.ToString();
            TextBoxOvertime.Text = user.OverTime.ToString();
            TextBoxVacation.Text = user.VacationDays.ToString();
            TextBoxCurrentVacation.Text = user.CurrentVacationDays.ToString();
        }

        //Click listener when project item is clicked in project list
        //App navigates to recording pivot item and displayes previous sessions
        /**
         * 
         */
        private void Tap_ProjectItem(object sender, GestureEventArgs e)
        {
            var button = sender as TextBlock;
            ProjectItem clickedProjectItem = button.DataContext as ProjectItem;
            _currentSessionItem = new SessionItem(clickedProjectItem.ProjectId);
            _dataBaseManager.LoadCurrentSessions(clickedProjectItem.ProjectId);
            TextBlockCurrentProject.Text = clickedProjectItem.ProjectName;
            CurrentSessionItems = _dataBaseManager.CurrentSessionItems;
            CurrentSessionItems = Utils.ReverseCurrentSessionItems(CurrentSessionItems);
            CurrentSessionList.ItemsSource = CurrentSessionItems;
            PivotMain.SelectedIndex = 1;
        }
    }
}