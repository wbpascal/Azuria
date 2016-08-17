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

namespace Azuria.Example.Android
{
    [Activity(Label = "NotificationActivity")]
    public class NotificationActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Notification);

            SenpaiParcelable lSenpaiParcelable = this.Intent.GetParcelableExtra("SenpaiParcelable") as SenpaiParcelable;
        }
    }
}