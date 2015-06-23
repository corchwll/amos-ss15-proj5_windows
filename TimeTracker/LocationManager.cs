using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TimeTracker
{

    class LocationManager
    {
        uint _desireAccuracyInMetersValue = 10;
        private Geolocator _geolocator;
        public LocationManager()
        {
            _geolocator = new Geolocator();
            
        }

        public async void GetCurrentLocation()
        {
            Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = _desireAccuracyInMetersValue };
            //_geolocator.StatusChanged += OnStatusChanged;
            Geoposition pos = await geolocator.GetGeopositionAsync();

        }

    }
}
