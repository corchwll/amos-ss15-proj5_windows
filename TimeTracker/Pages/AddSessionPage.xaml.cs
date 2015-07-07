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
using System.Windows;
using Microsoft.Phone.Controls;

namespace TimeTracker
{
    public partial class AddSessionPage : PhoneApplicationPage
    {
        string _projectId;
        public AddSessionPage()
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
                MessageBoxResult result = MessageBox.Show("Negative times are not allowed",
                    "Error", MessageBoxButton.OKCancel);

                return;
            }
            NavigationService.Navigate(new Uri("/Pages/MainPivotPage.xaml?start=" + timestampStart + "&" + "end=" + timestampEnd + "&" + "id=" + _projectId, UriKind.Relative));

            
        }
    }   
}