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
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.Devices.Geolocation;

namespace TimeTracker
{

    class LocationManager
    {
        uint _desireAccuracyInMetersValue = 50;
        private Geoposition _currentGeoposition;
        private DispatcherTimer _dispatcherTimer;

        private Geolocator _geolocator;
        public LocationManager()
        {
            InitTimer();
            _dispatcherTimer.Start();
            CheckPermission();
        }

        private void CheckPermission()
        {
            _geolocator = new Geolocator { DesiredAccuracyInMeters = _desireAccuracyInMetersValue };
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
            {
                // User has opted in or out of Location
            }
            else
            {
                MessageBoxResult result =
                    MessageBox.Show("This app accesses your phone's location. Is that ok?",
                    "Location",
                    MessageBoxButton.OKCancel);

                if (result == MessageBoxResult.OK)
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }

                IsolatedStorageSettings.ApplicationSettings.Save();
            }
        }

        public bool DidLocationChanged(Geoposition newPosition)
        {
            if (_currentGeoposition == null)
            {
                _currentGeoposition = newPosition;
                return true;

            }

            if (Math.Round(newPosition.Coordinate.Latitude, 5) ==
                Math.Round(_currentGeoposition.Coordinate.Latitude, 5)
                &&
                Math.Round(newPosition.Coordinate.Longitude, 5) ==
                Math.Round(_currentGeoposition.Coordinate.Longitude, 5))
            {
                return false;
            }

            _currentGeoposition = newPosition;
                return true;
        }

        public Geoposition GetCurrentGeoposition()
        {
            return _currentGeoposition;
        }

        public async Task<Geoposition> LoadLocation()
        {

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
            {
                // The user has opted out of Location.
                return null;
            }
            try
            {
                Geoposition geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10)
                    );

                if(DidLocationChanged(geoposition))
                {
                    _currentGeoposition = geoposition;
                }

                return geoposition;
            }
            catch (Exception ex)
            {

                    Debug.WriteLine("Exception while getting geolocation");
                return null;
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
            LoadLocation();
            if (_currentGeoposition != null)
            {
                _dispatcherTimer.Stop();

            }

        }

        private async void OnStatusChanged(Geolocator sender, StatusChangedEventArgs e)
        {
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //});
        }

    }
}
