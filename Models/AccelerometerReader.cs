using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace AppVoice.Models
{
    class AccelerometerReader
    {
        public SensorSpeed Speed = SensorSpeed.UI;
        public float AccX { get; set; }
        public float AccY { get; set; }
        public float AccZ { get; set; }
        public bool Started { get; set; }
        public bool Shaked { get; set; }

        public AccelerometerReader()
        {
            this.AccX = 0;
            this.AccY = 0;
            this.AccZ = 0;
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
            this.Started = false;
            this.Shaked = false;
        }
        public void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            this.AccX = data.Acceleration.X;
            this.AccY = data.Acceleration.Y;
            this.AccZ = data.Acceleration.Z;
            //Console.WriteLine($"Accelerometer: X: {data.Acceleration.X}, Y: {data.Acceleration.Y}, Z: {data.Acceleration.Z}");
        }


        public float Test()
        {
            return this.AccX;
        }

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            this.Shaked = true;
            Console.WriteLine("SHAKE");
        }
        public void ToggleAccelerometer()
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                    this.Started = false;
                    Console.WriteLine("Accelerometer is stopped");
                } else
                {
                    Console.WriteLine("Accelerometer is started");
                    Accelerometer.Start(Speed);
                    this.Started = true;
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Console.WriteLine("Non Pris en charge" + fnsEx);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une Exception s'est produite" + ex);
            }
        }
    }
}