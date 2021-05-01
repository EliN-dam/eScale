using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Plugin.Messaging;
using System.Threading.Tasks;

namespace eScale
{
    public class Notifications
    {
        public static async Task SendSMS(string phone, string message)
        {
            PermissionStatus status = await CheckAndRequestPermissions();
            if (status != PermissionStatus.Granted) { return; }
            ISmsTask smsMessenger = CrossMessaging.Current.SmsMessenger;
            if (smsMessenger.CanSendSmsInBackground)
            {
                smsMessenger.SendSmsInBackground(phone, message);
            }
        }

        private static async Task<PermissionStatus> CheckAndRequestPermissions()
        {
            PermissionStatus status = await new Permissions.Sms().CheckStatusAsync();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Sms>();
            }
            return status;
        }
    }
}
