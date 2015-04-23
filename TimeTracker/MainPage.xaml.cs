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
        Boolean isTimerRunning = false;

        private int currentTimestampStart;
        private int currentTimestampStop;


        private SessionDataContext sessionDB;

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

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            InitTimer();

            sessionDB = new SessionDataContext(SessionDataContext.DBConnectionString);

            this.DataContext = this;
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
            textBoxTime.Text = "" + getMinutes(totalSeconds) + ":" + getSeconds(totalSeconds);

        }

        //called when start/stop recording is clicked to start/stop the dispatcherTimer instance
        private void startRecording_Click(object sender, RoutedEventArgs e)
        {
            //if the timer was not started yet it will get started and the button content will get changed
            if(!isTimerRunning){
                currentTimestampStart = getUnixTimestamp();
                totalSeconds = 0;
                textBoxTime.Text = "00:00";
                isTimerRunning = true;
                dispatcherTimer.Start();
                buttonStartRecording.Content = "Stop recording";
            }
            //if the timer is already running, it will get stopoped, the button content will get changed and
            //the output is showen in the UI
            else{
                currentTimestampStop = getUnixTimestamp();
                isTimerRunning = false;
                dispatcherTimer.Stop();
                textBoxTime.Text = "" + getMinutes(totalSeconds) + ":" + getSeconds(totalSeconds);
                buttonStartRecording.Content = "Start recording";
                createNewSessionItem("projectX", currentTimestampStart, currentTimestampStop);
                saveChangesToDatabase();
                testQueryDatabase();
            }
        }

        private void saveData_Click(object sender, RoutedEventArgs e)
        {
        }

        private void startRecording_Click(object sender, RoutedEventArgs e)
        {
        }


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

        public void createNewSessionItem(string projectId, int timestampStart, int timestampStop)
        {
            SessionItem newSession = new SessionItem { ProjectId = projectId, TimestampStart = timestampStart, TimestampStop = timestampStop};
            sessionDB.SessionItems.InsertOnSubmit(newSession);
        }

        public void saveChangesToDatabase()
        {
            sessionDB.SubmitChanges();
        }

        private void testQueryDatabase()
        {
            var sessionItemsInDB = from SessionItem todo in sessionDB.SessionItems select todo;
            SessionItems = new ObservableCollection<SessionItem>(sessionItemsInDB);
            foreach (var item in SessionItems)
            {
                Debug.WriteLine(item.TimestampStart);
            }

        }
    }

    public class SessionDataContext : DataContext
    {
        public static string DBConnectionString = "Data Source=isostore:Session.sdf";


        public SessionDataContext(string connectionString) : base(connectionString)
        { }

        public Table<SessionItem> SessionItems;
    }
}