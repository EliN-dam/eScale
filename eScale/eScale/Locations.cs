using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace eScale
{
    public class Locations
    {
        public static async Task<Location> GetCurrentLocation()
        {
            PermissionStatus status = await CheckAndRequestPermissions();
            if (status != PermissionStatus.Granted) { return null; }
            return await Geolocation.GetLocationAsync();
        }

        public static async Task<Location> GetLastKnownLocation()
        {
            PermissionStatus status = await CheckAndRequestPermissions();
            if (status != PermissionStatus.Granted) { return null; }
            return await Geolocation.GetLastKnownLocationAsync();
        }

        private static async Task<PermissionStatus> CheckAndRequestPermissions()
        {
            PermissionStatus status = await new Permissions.Sms().CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
            return status;
        }
    }
}
