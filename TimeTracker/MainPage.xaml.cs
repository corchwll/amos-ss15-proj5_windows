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
using Microsoft.Phone.Controls;

namespace TimeTracker
{
    public partial class MainPage : PhoneApplicationPage
    {

        Boolean isTimerRunning = false;
        Int32 startingTime;
        Int32 endingTime;

        // Konstruktor
        public MainPage()
        {

            InitializeComponent();
        }

        private void startRecording_Click(object sender, RoutedEventArgs e)
        {
            
            if(!isTimerRunning){
                startingTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970,1,1))).TotalSeconds;
                isTimerRunning = true;
                textBoxTime.Text = "START";
            }else{
                isTimerRunning = false;
                endingTime = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                long dif = endingTime - startingTime;
                textBoxTime.Text = "" + dif;
            }
        }
    }
}