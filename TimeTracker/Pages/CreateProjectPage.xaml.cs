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
using System.Windows.Threading;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Controls;

namespace TimeTracker.Pages
{
    /**
     * This Page shows the user a form which has to be filled with project
     * information. As soon as the information is confirmed it gets send
     * back to the MainPivotPage.xaml
     */
    public partial class CreateProjectPage : PhoneApplicationPage
    {


        private DispatcherTimer _dispatcherTimer;
        private Geoposition _position;
        readonly LocationManager lManager = new LocationManager();

        public CreateProjectPage()
        {
            InitializeComponent();
            lManager.LoadLocation();
            InitTimer();
            _dispatcherTimer.Start();

        }


        private void Click_CreateProject(object sender, RoutedEventArgs e)
        {

            string projectName = TextBoxName.Text;
            string projectId = TextBoxId.Text;
            DateTime selectedTime = ((DateTime) FinalDate.Value);
            int date = Utils.TotalSeconds(selectedTime);
            string latitude = TextBoxLatitude.Text;
            string longitude = TextBoxLongitude.Text;

            if (!ProjectItem.CheckProjectId(projectId))
            {
                MessageBoxResult result = MessageBox.Show("Your project ID must consist of 5 digits",
                      "Error", MessageBoxButton.OKCancel);
                return;
            }

            if (latitude.Length < 5 || longitude.Length < 5)
            {
                MessageBoxResult result = MessageBox.Show("Add the location of your project",
                      "Error", MessageBoxButton.OKCancel);
                return;
            }



            UriFactory factory = new UriFactory();
            NavigationService.Navigate(new Uri(factory.CreateProjectDataUri(projectName, projectId,
                                        date.ToString(), latitude, longitude), UriKind.Relative));

        }

        private int convertTicksToUnixTimestamp(int ticks)
        {
            int epochTicks = (int) new DateTime(1970, 1, 1).Ticks;
            int timestamp = (int) ((ticks - epochTicks)/TimeSpan.TicksPerSecond);
            return timestamp;
        }


        private void GetLocation_Click(object sender, RoutedEventArgs e)
        {
            if (_position != null)
            {
                TextBoxLatitude.Text = _position.Coordinate.Latitude.ToString("0.0000");
                TextBoxLongitude.Text = _position.Coordinate.Longitude.ToString("0.0000");

            }
        }

        private void InitTimer()
        {
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
        }

        //EventHandler for each timer tick. Updates the textBox with the current time passed
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (_position == null)
            {
                var position = lManager.GetCurrentGeoposition();
                if (position != null)
                {
                    _position = position;

                }

            }
            
        }


    }
}