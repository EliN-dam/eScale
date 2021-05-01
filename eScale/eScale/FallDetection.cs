using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace eScale
{
    public class FallDetection
    {
        public event Action FallEvent;
        public double Threshold { get; set; }
        private int SecurityCount;

        public FallDetection()
        {
            SecurityCount = 0;
        }

        public void Toggle()
        {
            if (Accelerometer.IsMonitoring) { Start(); }
            else { Stop(); }
        }

        public void Start()
        {
            Accelerometer.ReadingChanged += DetectionEvent;
            Accelerometer.Start(SensorSpeed.Game);
        }

        public void Stop()
        {
            Accelerometer.ReadingChanged -= DetectionEvent;
            Accelerometer.Stop();
        }

        public void DetectionEvent(object sender, AccelerometerChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                AccelerometerData data = e.Reading;
                if (FallDetected(data))
                {
                    SecurityCount++;
                }
                if (SecurityCount >= 3)
                {
                    SecurityCount = 0;
                    FallEvent?.Invoke();
                }
            });
            
        }

        private bool FallDetected(AccelerometerData data)
        {
            double acceleration = Math.Sqrt(Math.Pow(data.Acceleration.X, 2) + Math.Pow(data.Acceleration.Y, 2) + Math.Pow(data.Acceleration.Z, 2));
            return acceleration > Threshold;
        }
    }
}
