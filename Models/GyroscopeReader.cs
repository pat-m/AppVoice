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
    class GyroscopeReader
    {
        public SensorSpeed Speed = SensorSpeed.UI;
        public float GyrX { get; set; }
        public float GyrY { get; set; }
        public float GyrZ { get; set; }
        public bool Started { get; set; }

        public GyroscopeReader()
        {
            Gyroscope.ReadingChanged += Gyroscope_ReadingChanged;
        }

        void Gyroscope_ReadingChanged(object sender, GyroscopeChangedEventArgs e)
        {
            var data = e.Reading;
            this.GyrX = data.AngularVelocity.X;
            this.GyrY = data.AngularVelocity.Y;
            this.GyrZ = data.AngularVelocity.Z;
            Console.WriteLine($"Gyroscope Reading: X: {data.AngularVelocity.X}, Y: {data.AngularVelocity.Y}, Z: {data.AngularVelocity.Z}");
        }

        public void ToggleGyroscope()
        {
            try
            {
                if (Gyroscope.IsMonitoring)
                {
                    Gyroscope.Stop();
                    this.Started = false;
                    Console.WriteLine("Gyroscope is stoped");
                }
                else
                {
                    Gyroscope.Start(Speed);
                    this.Started = true;
                    Console.WriteLine("Gyroscope is started");
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