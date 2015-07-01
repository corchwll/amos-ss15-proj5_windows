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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using TimeTracker.BusinessLogic;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace TimeTracker
{
    public partial class MainPivotPage : PhoneApplicationPage
    {

        #region Global Variables
        /**
         * 
         */
        private SessionItem _currentSessionItem;

        private DashboardInformation _dashboardInformation;

        //Timer instance for recording sessions and update UI
        private DispatcherTimer _dispatcherTimer;

        //Database instance - Create, Delete, Update or Remove elements
        private readonly DatabaseManager _dataBaseManager;

        private UserItem _currentUser;

        readonly LocationManager _locationManager = new LocationManager();




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

        #endregion

        #region Page Lifecycle Methods

        //Lifecycle method when a certain pivot item is loaded
        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDashboard();
            
        }

        private void UpdateDashboard()
        {
            if (_dataBaseManager.UserItems.Count > 0 && SessionItems != null)
            {
                _dashboardInformation = new DashboardInformation(SessionItems, _dataBaseManager.UserItems.First());
                TextBlockDashboardVacation.Text = _dashboardInformation.LeftVacationDays().ToString();

                TextBlockDashboardOvertime.Text = _dashboardInformation.CalculateOvertime(SessionItems.ToList(), _dataBaseManager.UserItems.First()).ToString();

            }
        }

        // Konstruktor
        public MainPivotPage()
        {
            InitializeComponent();
            InitTimer();
            DataContext = this;
            _dataBaseManager = new DatabaseManager();
            LoadData();
            if (_dataBaseManager.UserItems.Count > 0)
            {
                _dashboardInformation = new DashboardInformation(SessionItems, _dataBaseManager.UserItems.First());
            }
            SortProjectItemCollectionByPosition();
            ResetVacationDays();
            PutExpirationWarning();
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

        public void LoadData()
        {
            LoadUser();
            ProjectItems = _dataBaseManager.ProjectItems;
            SortProjectItemCollectionByPosition();
            CurrentSessionItems = _dataBaseManager.CurrentSessionItems;
            SessionItems = _dataBaseManager.SessionItems;
            
        }

        private void LoadUser()
        {
            if (_dataBaseManager.UserItems.Count == 0)
            {
                return;
            }
            _currentUser = _dataBaseManager.UserItems[0];
        }

        private void ResetVacationDays()
        {
            if (_currentUser == null)
            {
                return;
            }
            int year = Utils.GetDateTimeObject(_currentUser.LastVacationReset).Year;
            if (DateTime.Today.Year > year && DateTime.Today.Month >= 4)
            {
                _currentUser.VacationDays = 20;
                _currentUser.CurrentVacationDays = 0;
                _currentUser.LastVacationReset = DateTime.Today.Year;
                _dataBaseManager.UpdateUser(_currentUser);
            }
        }

        private void PutExpirationWarning()
        {
            if (DateTime.Today.Month < 4 &&  _currentUser != null && 
                (_currentUser.VacationDays - _currentUser.CurrentVacationDays) > 0)
            {
                ExpireWarningTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                ExpireWarningTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void SortProjectItemsCollection()
        {
            ProjectItems = new ObservableCollection<ProjectItem>(ProjectItems.OrderBy(a => a.ProjectName));
           
            RearrangeDefaultProject(DatabaseManager.ProjectTrainingId);
            RearrangeDefaultProject(DatabaseManager.ProjectOfficeId);
            RearrangeDefaultProject(DatabaseManager.ProjectIllnessId);
            RearrangeDefaultProject(DatabaseManager.ProjectHolidayId);
        }

        private void SortProjectItemCollectionByPosition()
        {

            if (_currentUser == null || !_currentUser.SortByLocation)
            {
                SortProjectItemsCollection();
                return;

            }
            
            Geoposition position = _locationManager.GetCurrentGeoposition();
            double latitude;
            double longitude;
            if (position != null)
            {
                _currentUser.Latitude = position.Coordinate.Latitude;
                _currentUser.Longitude = position.Coordinate.Longitude;

                UserItem newUser = new UserItem
                {
                    Name = _currentUser.Name,
                    PersonalId = _currentUser.PersonalId,
                    Surname =  _currentUser.Surname,
                    CurrentVacationDays =  _currentUser.CurrentVacationDays,
                    Latitude = _currentUser.Latitude,
                    Longitude = _currentUser.Longitude,
                    OverTime = _currentUser.OverTime,
                    SortByLocation = _currentUser.SortByLocation,
                    VacationDays = _currentUser.VacationDays,

                };

                _dataBaseManager.UpdateUser(newUser);
            }
            else if (_currentUser.Latitude == null)
            {
                SortProjectItemsCollection();
                return;
            }


            latitude = _currentUser.Latitude;
            longitude = _currentUser.Longitude;
            foreach (var item in ProjectItems)
            {
                var currentCoordinate = new GeoCoordinate(latitude, longitude);
                var itemCoordinate = new GeoCoordinate(item.Latitude, item.Longitude);

                item.Distance = (int) currentCoordinate.GetDistanceTo(itemCoordinate);
                
            }
            ProjectItems = new ObservableCollection<ProjectItem>(ProjectItems.OrderBy(a => a.Distance));

            RearrangeDefaultProject(DatabaseManager.ProjectTrainingId);
            RearrangeDefaultProject(DatabaseManager.ProjectOfficeId);
            RearrangeDefaultProject(DatabaseManager.ProjectIllnessId);
            RearrangeDefaultProject(DatabaseManager.ProjectHolidayId);


            foreach (var item in ProjectItems)
            {
                Debug.WriteLine(item.ProjectName);

            }
        }

        public void RearrangeDefaultProject(string id)
        {
            IEnumerable<ProjectItem> items = ProjectItems.Where(a =>
                a.ProjectId == id);
            ProjectItem item = items.First();
            ProjectItems.Remove(item);
            ProjectItems.Insert(0, item);
        }
        /**
         * The following region checks if certain data was passed while
         * navigating to this page. If this is the case the data is collected
         * and appropriate methods are executed to create new items in the
         * database (User, Projects or Sessions)
         */
        #region Collecting Data
        //Catches necessary data and creates new project items in database
        //when create project interactions was executed
        private void CollectNewProject()
        {
            string tmp = "";
            if (NavigationContext.QueryString.TryGetValue("projectId", out tmp))
            {
                string id = CollectStringOnNavigation(QueryDictionary.ProjectId);
                string name = CollectStringOnNavigation(QueryDictionary.ProjectName);
                int finalDate = CollectIntOnNavgation(QueryDictionary.ProjectFinalDate);
                double latitude = CollectDoubleOnNavigation(QueryDictionary.ProjectLatitude);
                double longitude = CollectDoubleOnNavigation(QueryDictionary.ProjectLongitude);


                ProjectItem newItem =_dataBaseManager.createNewProjectItem(id, name, finalDate, latitude, longitude);
                if (ProjectItems.Count(x => x.ProjectId == id) > 0)
                {
                    IEnumerable<ProjectItem> comparingItems = ProjectItems.Where(x => x.ProjectId == id);
                    if (comparingItems.ElementAt(0) == newItem){}
                    {
                        return;
                    }
                    //_dataBaseManager.UpdateProject(newItem);
                    PivotMain.SelectedIndex = 2;
                }
                else
                {
                    ProjectItems.Add(newItem);
                    PivotMain.SelectedIndex = 2; 
                }   
            }
        }

        //Catches necessary data and creates new session items in database
        //when edit project was executed
        private void CollectNewSession()
        {
            string tmp = "";
            if (NavigationContext.QueryString.TryGetValue("start", out tmp))
            {
                int startConverted = CollectIntOnNavgation(QueryDictionary.SessionStart);
                int endConverted = CollectIntOnNavgation(QueryDictionary.SessionStop);
                string id = CollectStringOnNavigation(QueryDictionary.SessionProjectId);
                if (_dataBaseManager.IsMaxDayTimeReached(startConverted, endConverted))
                {
                    MessageBoxResult result = MessageBox.Show("The total working hours can not exeed ten hours per day",
                        "Error", MessageBoxButton.OKCancel);
                }
                else if (!_dataBaseManager.CreateNewSessionItem(id, startConverted, endConverted))
                {
                    MessageBoxResult result = MessageBox.Show("You have already recorded this time",
                        "Error", MessageBoxButton.OKCancel);
                }
                
            }


        }

        //Catches necessary data and creates new user item in database
        //when registration was executed
        private void CollectRegistrationData()
        {
            string name = "";
            int year;
            if (DateTime.Today.Month < 4)
            {
                year = DateTime.Today.Year - 1;
            }
            else
            {
                year = DateTime.Today.Year;
            }
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
                    CurrentVacationDays = CollectIntOnNavgation(QueryDictionary.UserCurrentVacationDaysKey),
                    SortByLocation =  CollectBoolOnNavigation("sortbylocation"),
                    LastVacationReset = year

                };
                _dataBaseManager.createNewUserItem(newUser);
                _currentUser = newUser;
                FillPersonalData(newUser);
                _dashboardInformation = new DashboardInformation(SessionItems, _dataBaseManager.UserItems.First());


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

        private double CollectDoubleOnNavigation(string key)
        {
            string result = "";
            NavigationContext.QueryString.TryGetValue(key, out result);
            
            double fin = Double.Parse(result);
            return fin;

        }

        private bool CollectBoolOnNavigation(string key)
        {
            string result = "";
            NavigationContext.QueryString.TryGetValue(key, out result);
            Debug.WriteLine(result);
            if (result.Equals("True"))
            {
                return true;
            }

            return false;
        }

        #endregion

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
            _currentSessionItem.TimestampStop = Utils.GetUnixTimestamp();
            TextBlockCurrentTimer.Text = Utils.FormatSecondsToChronometerString(_currentSessionItem.TotalTime);

            if (_currentSessionItem.TotalTime%60 == 0 || _currentSessionItem.Longitude == 0.0)
            {
                _locationManager.LoadLocation();
                Geoposition position = _locationManager.GetCurrentGeoposition();
                if (position != null)
                {
                    double latitude = Math.Round(position.Coordinate.Latitude, 4);
                    double longitude = Math.Round(position.Coordinate.Longitude, 4);
                    _currentSessionItem.AddPosition(longitude, latitude);
                    Debug.WriteLine(_currentSessionItem.Longitude.ToString());
                }
            }

            if (_dataBaseManager.IsMaxDayTimeReached(_currentSessionItem))
            {
                MessageBoxResult result = MessageBox.Show("The total working hours can not exeed ten hours per day",
                    "Error", MessageBoxButton.OKCancel);
            }
        }

        //following methods build the event callback functionalitiesof the buttons
        //implemented in the UI

        #region Clicklisteners

        //Click listener when user wants to start recording a new session for a project.
        //Saves the unix timestamp from the start and ending point and creates a new
        //session in the database.
        private void RecordingProject_Click(object sender, RoutedEventArgs e)
        {
            if (!_dispatcherTimer.IsEnabled)
            {
                StartRecording();
                return;
            }
            StopRecording();
        }

        private void StartRecording()
        {
            ProjectItem currentProjectItem =ProjectItems.Where(x => x.ProjectId == _currentSessionItem.ProjectId).ElementAt(0);
            int now = Utils.TotalSeconds(DateTime.Now.AddDays(1.0));
            if (now > currentProjectItem.FinaleDate)
            {
                MessageBoxResult result = MessageBox.Show("It is not possible to record times after the final project date",
                    "Error", MessageBoxButton.OKCancel);
            }
            else
            {
                _currentSessionItem.TimestampStart = Utils.GetUnixTimestamp();
                TextBlockCurrentTimer.Text = Utils.FormatSecondsToChronometerString(0);
                _dispatcherTimer.Start();
                ButtonStartStopRecording.Content = "Stop Recording";          
            }
            
        }

        private void StopRecording()
        {
            _currentSessionItem.TimestampStop = Utils.GetUnixTimestamp();
            _dispatcherTimer.Stop();
            TextBlockCurrentTimer.Text = Utils.FormatSecondsToChronometerString(_currentSessionItem.TotalTime);
            ButtonStartStopRecording.Content = "Start Recording";
            _dataBaseManager.CreateNewSessionItem(_currentSessionItem);
            UpdateDashboard();


        }

        //Click listener when user wants to create a new project
        //App navigates to the create project page
        private void newProject_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/CreateProjectPage.xaml", UriKind.Relative));
        }

        //Click listener when user wants to edit a project (add new sessions)
        //App navigates to the edit project page

        private void addSession_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;


            ProjectItem projectItem = menu.DataContext as ProjectItem;
            string projectId = projectItem.ProjectId;
            string projectName = projectItem.ProjectName;
            NavigationService.Navigate(new Uri("/Pages/AddSessionPage.xaml?id=" + projectId, UriKind.Relative));

        }


        private void editProject_Click(object sender, RoutedEventArgs e)
        {

            var menu = sender as MenuItem;


            ProjectItem projectItem = menu.DataContext as ProjectItem;
            string projectId = projectItem.ProjectId;
            string projectName = projectItem.ProjectName;
            double longitude = projectItem.Longitude;
            double latitude = projectItem.Latitude;
            int finalDate = projectItem.FinaleDate;

            UriFactory factory = new UriFactory();
            factory.SetNavigationBody("/Pages/EditProjectPage.xaml?");
            string uri = factory.CreateDataUri(projectName, projectId, finalDate.ToString(), latitude.ToString(),
                longitude.ToString());


            NavigationService.Navigate(new Uri(uri, UriKind.Relative));

        }

        //Click listener when change settings is executed.
        //User item is updated in database
        private void SavePersonalData_Click(object sender, RoutedEventArgs e)
        {

            Geoposition position = _locationManager.GetCurrentGeoposition();

           
            UserItem newUser = new UserItem
            {
                Name = TextBoxName.Text,
                Surname = TextBoxSurname.Text,
                PersonalId = TextBoxPersonalId.Text,
                WorkingTime = Int32.Parse(TextBoxHoursWeek.Text),
                OverTime = Int32.Parse(TextBoxOvertime.Text),
                VacationDays = Int32.Parse(TextBoxVacation.Text),
                CurrentVacationDays = Int32.Parse(TextBoxCurrentVacation.Text),
                SortByLocation = CheckBoxSortBy.IsChecked.Value
            };

            if (position != null)
            {
                newUser.Latitude = position.Coordinate.Latitude;
                newUser.Longitude = position.Coordinate.Longitude;
            }
            else if(_currentUser != null && _currentUser.Latitude != null)
            {
                newUser.Latitude = _currentUser.Latitude;
                newUser.Longitude = _currentUser.Longitude;
            }
            _currentUser = newUser;
            _dataBaseManager.UpdateUser(newUser);
            FillPersonalData(newUser);
            SortProjectItemCollectionByPosition();
        }

        //Deletes a project after the user has acknowledged the task
        private void deleteProject_Click(object sender, RoutedEventArgs e)
        {
            var menu = sender as MenuItem;
            ProjectItem projectItem = menu.DataContext as ProjectItem;

            MessageBoxResult result = MessageBox.Show("Are you sure?",
                      "Deleting project", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            ProjectItems.Remove(projectItem);
            _dataBaseManager.DeleteProject(projectItem);
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
            CheckBoxSortBy.IsChecked = user.SortByLocation;
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

        private void TextBoxSearchProject_OnKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            string input = textbox.Text;
            
            Debug.WriteLine(input);
            if (input.Length == 0)
            {
                //ProjectItems = _dataBaseManager.ProjectItems;
                //return;
            }
            ProjectItems.Clear();

            List<ProjectItem> items = _dataBaseManager.ProjectItems.Where(item => item.ProjectName.Contains(input)).ToList();
            List<ProjectItem> items2 = _dataBaseManager.ProjectItems.Where(item => item.ProjectId.Contains(input)).ToList();

            foreach (ProjectItem item in items)
            {
                ProjectItems.Add(item);
            }
            foreach (ProjectItem item in items2)
            {
                if (!ProjectItems.Contains(item))
                {
                    ProjectItems.Add(item);
                }
            }

        }

        private void ButtonSessionDelete_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;


            SessionItem item = button.DataContext as SessionItem;

            MessageBoxResult result = MessageBox.Show("Are you sure?",
                      "Deleting session", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Cancel)
            {
                return;
            }
            SessionItems.Remove(item);
            CurrentSessionItems.Remove(item);
            _dataBaseManager.DeleteSession(item);


        }

        private void Export_OnClick(object sender, RoutedEventArgs e)
        {
            CsvFactory factory = new CsvFactory(SessionItems.ToList(), ProjectItems.ToList(), _dataBaseManager.UserItems.First());
            factory.CreateCsvFile();
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "message subject";
            emailComposeTask.Body = factory.CreateCsvAsString();
            emailComposeTask.To = "daniel.lenerd.lohse@gmail.com";
            
            emailComposeTask.Show();
        }

        private void ReadButton_OnClick(object sender, RoutedEventArgs e)
        {
            CsvFactory factory = new CsvFactory(null, null, null);
            factory.DebugFile();
        }
    }
}