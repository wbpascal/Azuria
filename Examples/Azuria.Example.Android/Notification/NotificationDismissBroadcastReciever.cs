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

namespace Azuria.Example.Android.Notification
{
    public class NotificationDismissBroadcastReciever : BroadcastReceiver
    {
        public static NotificationService NotificationService;

        public override void OnReceive(Context context, Intent intent)
        {
            var debug = true;
        }
    }
}