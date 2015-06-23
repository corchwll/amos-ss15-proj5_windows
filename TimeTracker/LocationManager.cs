using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Windows.Devices.Geolocation;
using Windows.UI.Core;

namespace TimeTracker
{

    class LocationManager
    {
        uint _desireAccuracyInMetersValue = 50;
        private Geoposition _currentGeoposition;
        private Geolocator _geolocator;
        public LocationManager()
        {
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

        public async void LoadLocation()
        {

            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;
            if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
            {
                // The user has opted out of Location.
                return;
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
            }
            catch (Exception ex)
            {

                    Debug.WriteLine("Exception while getting geolocation");
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
