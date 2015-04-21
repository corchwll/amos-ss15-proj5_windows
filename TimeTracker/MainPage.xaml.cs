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

namespace TimeTracker
{

    public partial class MainPage : PhoneApplicationPage
    {
        private DispatcherTimer dispatcherTimer;
        private Int32 totalSeconds;
        Boolean isTimerRunning = false;
        Int32 startingTime;
        Int32 endingTime;

        // Konstruktor
        public MainPage()
        {
            InitializeComponent();
            InitTimer();
        }

        private void InitTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            totalSeconds++;
            textBoxTime.Text = "" + getMinutes(totalSeconds) + ":" + getSeconds(totalSeconds);

        }

        //called when start/stop recording is clicked to calculate the time the user is working on a project
        private void startRecording_Click(object sender, RoutedEventArgs e)
        {
            //if the timer was not started yet, the starting time is saved and the button content is changed
            if(!isTimerRunning){
                totalSeconds = 0;
                textBoxTime.Text = "00:00";
                isTimerRunning = true;
                dispatcherTimer.Start();
                //startingTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970,1,1))).TotalSeconds;
                buttonStartRecording.Content = "Stop recording";
            }
            //if the timer is already running, the ending time is saved, the working time is calculated and
            //the output is showen in the UI
            else{
                isTimerRunning = false;
                //endingTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                //Int32 dif = endingTime - startingTime;
                dispatcherTimer.Stop();
                textBoxTime.Text = "" + getMinutes(totalSeconds) + ":" + getSeconds(totalSeconds);
                buttonStartRecording.Content = "Start recording";
            }
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






    }
}