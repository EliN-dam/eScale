using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace eScale
{
    public class Flashlight
    {
        public bool FlashStatus { get; set; }

        public async Task Toggle()
        {
            if (FlashStatus) { await TurnOff(); }
            else { await TurnOn(); }
        }

        public async Task TurnOn()
        {
            PermissionStatus status = await CheckAndRequestPermissions();
            if (status != PermissionStatus.Granted) { return; }
            try
            {
                await Xamarin.Essentials.Flashlight.TurnOnAsync();
                FlashStatus = true;
            }
            catch (FeatureNotSupportedException ex) { }
        }

        public async Task TurnOff()
        {
            await Xamarin.Essentials.Flashlight.TurnOffAsync();
            FlashStatus = false;
        }

        private async Task<PermissionStatus> CheckAndRequestPermissions()
        {
            PermissionStatus status = await new Permissions.Sms().CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Flashlight>();
            }
            return status;
        }
    }
}
