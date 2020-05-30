using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace AppVoice.Models
{
    class Vibrate
    {
        public void Start()
        {
            try
            {
                Vibration.Vibrate();
                var duration = TimeSpan.FromSeconds(10);
                Vibration.Vibrate(duration);
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine("Fonction non supportée" + ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une Exception s'est produite" + ex);
            }
        }

        public void Stop()
        {
            try
            {
                Vibration.Cancel();
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine("Fonction non supportée" + ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Une Exception s'est produite" + ex);
            }
        }
    }
}