using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Globalization;

namespace eScale
{
    public partial class MainPage : ContentPage
    {
        private FallDetection FallEvent;
        private Flashlight FlashService;
        private double FallThreshold;
        private bool FlashlightState;
        private bool MessagingState;

        public MainPage()
        {
            InitializeComponent();
            FlashService = new Flashlight();
            FallEvent = new FallDetection();
            SetFallThreshold();
            FallEvent.FallEvent += () => HasFallen();
        }

        private void FallToggle_Toggled(object sender, ToggledEventArgs e)
        {
            Switch button = sender as Switch;
            if (button.IsToggled)
            {
                FallEvent.Start();
                ThresholdTxt.IsEnabled = false;
            }
            else
            {
                FallEvent.Stop();
                ThresholdTxt.IsEnabled = true;
            }
        }

        private void SetFallThreshold()
        {
            if (Double.TryParse(ThresholdTxt.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out double value))
            {
                FallThreshold = value;
                FallEvent.Threshold = FallThreshold;
            }
            
        }

        private void ThresholdTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetFallThreshold();
        }

        private async void HasFallen()
        {
            FallEvent.Stop();
            FallToggle.IsToggled = false;
            //DisplayAlert("Mensaje", "El telefono se ha caido", "Aceptar");
            if (FlashlightState)
            {
                FlashService.TurnOn();
                FlashlightBtn.Text = "Apagar Linterna";
            }
            if (MessagingState)
            {
                if (!string.IsNullOrEmpty(PhoneTxt.Text))
                {
                    Notifications.SendSMS(PhoneTxt.Text, await GetSMS());
                }
            }
        }

        private async void FlashlightBtn_Clicked(object sender, EventArgs e)
        {
            await FlashService.Toggle();
            if (FlashService.FlashStatus) { FlashlightBtn.Text = "Apagar Linterna"; }
            else { FlashlightBtn.Text = "Encender Linterna"; }
        }

        private void FlashlightToggle_Toggled(object sender, ToggledEventArgs e)
        {
            FlashlightState = !FlashlightState;
        }

        private void SmsToggle_Toggled(object sender, ToggledEventArgs e)
        {
            MessagingState = !MessagingState;
            //PhoneLbl.IsVisible = !PhoneLbl.IsVisible;
            PhoneTxt.IsVisible = !PhoneTxt.IsVisible;
        }

        private async Task<string> GetSMS()
        {
            Location location = await Locations.GetCurrentLocation();
            return $"¡Mensaje de socorro! Posible accidente en las siguientes coordenadas: " +
                $"latitud {location.Latitude}, longitud: {location.Longitude}, altitud: {location.Altitude}. " +
                $"Mapa: https://www.google.com/maps/search/?api=1&query={location.Latitude.ToString(CultureInfo.InvariantCulture)}," +
                $"{location.Longitude.ToString(CultureInfo.InvariantCulture)}";
        }

        private void ConnectionStateBtn_Clicked(object sender, EventArgs e)
        {
            if (Connection.CheckConnection())
            {
                DisplayAlert("Estado de la conexión", "El dispositivo tiene acceso a Internet", "Aceptar");      
            }
            else
            {
                DisplayAlert("Estado de la conexión", "El dispositivo no tiene una conexión a Internet activa", "Aceptar");
            }
        }

        private void DebugToggle_Toggled(object sender, ToggledEventArgs e)
        {
            ThresholdTxt.IsVisible = !ThresholdTxt.IsVisible;
        }
    }
}
