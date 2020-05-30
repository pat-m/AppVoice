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
    class OrientationReader
    {

        public SensorSpeed Speed = SensorSpeed.UI;
        public float OrrX { get; set; }
        public float OrrY { get; set; }
        public float OrrZ { get; set; }
        public float OrrW { get; set; }
        public bool Started { get; set; }

        public OrientationReader()
        {
            OrientationSensor.ReadingChanged += OrientationSensor_ReadingChanged;
        }

        void OrientationSensor_ReadingChanged(object sender, OrientationSensorChangedEventArgs e)
        {
            var data = e.Reading;
            this.OrrX = data.Orientation.X;
            this.OrrY = data.Orientation.Y;
            this.OrrZ = data.Orientation.Z;
            this.OrrW = data.Orientation.W;
            Console.WriteLine($"Orientation: X: {data.Orientation.X}, Y: {data.Orientation.Y}, Z: {data.Orientation.Z}, W: {data.Orientation.W}");
        }

        public void ToggleOrientationSensor()
        {
            try
            {
                if (OrientationSensor.IsMonitoring)
                {
                    OrientationSensor.Stop();
                    this.Started = false;
                    Console.WriteLine("Orientation is stoped");
                }

                else
                {
                    OrientationSensor.Start(Speed);
                    this.Started = true;
                    Console.WriteLine("Orientation is started");
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