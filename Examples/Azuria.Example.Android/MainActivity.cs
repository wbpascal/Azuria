using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Azuria.Example.Android
{
    [Activity(Label = "MainActivity")]
    public class MainActivity : Activity
    {
        #region

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.SetContentView(Resource.Layout.Main);

            SenpaiParcelable lSenpaiParcelable = this.Intent.GetParcelableExtra("SenpaiParcelable") as SenpaiParcelable;

            this.FindViewById<TextView>(Resource.Id.UserIdView).Text = lSenpaiParcelable?.Senpai.Me?.Id.ToString();

            Button lStartAnimeMangaNotificationsButton =
                this.FindViewById<Button>(Resource.Id.StartAMNotificationsButton);
            var debug = true;
            lStartAnimeMangaNotificationsButton.Click += (sender, args) =>
            {
                Intent lNotificationActivity = new Intent(this, typeof(NotificationService));
                lNotificationActivity.PutExtra("SenpaiParcelable", lSenpaiParcelable);
                this.StartService(lNotificationActivity);
            };
        }

        #endregion
    }
}