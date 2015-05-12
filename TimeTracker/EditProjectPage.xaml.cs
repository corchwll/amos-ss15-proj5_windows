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

using System.Diagnostics;

namespace TimeTracker
{
    public partial class EditProjectPage : PhoneApplicationPage
    {
        string _projectId;
        public EditProjectPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string id = "";

            if (NavigationContext.QueryString.TryGetValue("id", out id))
            {
                ProjectName.Text = id;
                _projectId = id;
            }
        }

        private void onCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void onSave_Click(object sender, RoutedEventArgs e)
        {
            long epochTicks = new DateTime(1970, 1, 1).Ticks;
            DateTime date = WorkingDate.Value.Value.Date;
            TimeSpan startingTime = Startingtime.Value.Value.TimeOfDay;
            TimeSpan endingTime = EndingTime.Value.Value.TimeOfDay;
            int timestampStart = (int)(((date.Ticks - epochTicks)/TimeSpan.TicksPerSecond) + startingTime.TotalSeconds);
            int timestampEnd = (int)(((date.Ticks - epochTicks)/TimeSpan.TicksPerSecond) + endingTime.TotalSeconds);
            if (timestampEnd - timestampStart <= 0)
            {
                MessageBoxResult result = MessageBox.Show("The starting time is ahead of the ending time",
                    "Range not possible!", MessageBoxButton.OKCancel);

                return;
            }
            NavigationService.Navigate(new Uri("/MainPage.xaml?start=" + timestampStart + "&" + "end=" + timestampEnd + "&" + "id=" + _projectId, UriKind.Relative));

            
        }
    }   
}