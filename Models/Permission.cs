using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using static Xamarin.Essentials.Permissions;

namespace AppVoice.Models
{
    public class Permission : BasePermission
    {
        
        public override Task<PermissionStatus> CheckStatusAsync()
        {
            return Permissions.CheckStatusAsync<Permissions.Vibrate>();
        }
        public override void EnsureDeclared()
        {
            throw new System.NotImplementedException();
        }
        public override Task<PermissionStatus> RequestAsync()
        {
            return Permissions.RequestAsync<Permissions.Vibrate>();
        }
    }
}