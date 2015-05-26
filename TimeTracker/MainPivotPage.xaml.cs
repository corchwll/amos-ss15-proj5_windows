using System;
using System.Collections.ObjectModel;
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
        private DispatcherTimer _dispatcherTimer;
        private Int32 _totalSeconds;
        private Boolean _isTimerRunning = false;

        private int _currentTimestampStart;
        private int _currentTimestampStop;
        private string _currentProjectName;
        private string _currentProjectId;

        private readonly DatabaseManager _dataBaseManager;

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

        #region Page Lifecycle Methods

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


            }


        }

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

        private void CollectRegistrationData()
        {
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
                _dataBaseManager.createNewUserItem(name, surname, personalId, workingTime, overtime, vacationDays, currentVacation);
                UserItem newUser = new UserItem
                {
                    Name = name,
                    Surname = surname,
                    PersonalId = personalId,
                    WorkingTime = workingTime,
                    OverTime = overtime,
                    VacationDays = vacationDays,
                    CurrentVacationDays = currentVacation

                };
                FillPersonalData(newUser);

            }
            else if (!_dataBaseManager.userExists())
            {
                NavigationService.Navigate(new Uri("/RegistrationPage.xaml", UriKind.Relative));
            }
            
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
            _totalSeconds++;
            TextBoxTime.Text = "" + Utils.GetMinutes(_totalSeconds) + ":" + Utils.GetSeconds(_totalSeconds);

        }

        //following methods build the callback functionalities of the buttons
        //implemented in the UI

        #region Clicklisteners

        private void startRecordingProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                if (!_isTimerRunning)
                {
                    _currentTimestampStart = Utils.GetUnixTimestamp();
                    _totalSeconds = 0;
                    TextBoxTime.Text = "00:00";
                    _isTimerRunning = true;
                    _dispatcherTimer.Start();
                    button.Content = "stop";
                    ProjectItem projectItem = button.DataContext as ProjectItem;
                    _currentProjectId = projectItem.ProjectId;

                    _currentProjectName = projectItem.ProjectName;
                }
                //if the timer is already running, it will get stopoped, the button content will get changed and
                //the output is showen in the UI
                else
                {
                    _currentTimestampStop = Utils.GetUnixTimestamp();
                    _isTimerRunning = false;
                    _dispatcherTimer.Stop();
                    _totalSeconds = _currentTimestampStop - _currentTimestampStart;
                    TextBoxTime.Text = "" + Utils.GetMinutes(_totalSeconds) + ":" + Utils.GetSeconds(_totalSeconds);
                    button.Content = "start";
                    _dataBaseManager.CreateNewSessionItem(_currentProjectId, _currentTimestampStart, _currentTimestampStop);
                    _dataBaseManager.saveChangesToDatabase();
                }
            }
        }

        private void newProject_Click(object sender, RoutedEventArgs e)
        {

            NavigationService.Navigate(new Uri("/CreateProjectPage.xaml", UriKind.Relative));



        }

        private void queryData_Click(object sender, RoutedEventArgs e)
        {
            _dataBaseManager.testQueryDatabase();
        }

        private void editProject_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ProjectItem projectItem = button.DataContext as ProjectItem;
            string projectId = projectItem.ProjectId;
            string projectName = projectItem.ProjectName;
            NavigationService.Navigate(new Uri("/EditProjectPage.xaml?id=" + projectId, UriKind.Relative));
        }

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

        private void FillPersonalData()
        {
            if (_dataBaseManager.UserItems.Count == 0)
            {
                return;
            }
            UserItem user = _dataBaseManager.UserItems[0];

            FillPersonalData(user);
        }

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

        private void Tap_ProjectItem(object sender, GestureEventArgs e)
        {
            var button = sender as TextBlock;
            ProjectItem clickedProjectItem = button.DataContext as ProjectItem;
            TextBlockCurrentProject.Text = clickedProjectItem.ProjectName;
            PivotMain.SelectedIndex = 1;
        }
    }
}