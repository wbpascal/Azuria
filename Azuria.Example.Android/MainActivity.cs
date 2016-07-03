using Android.App;
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

            // Create your application here
            this.SetContentView(Resource.Layout.Main);

            Senpai lSenpai = (this.Intent.GetParcelableExtra("SenpaiParcelable") as SenpaiParcelable)?.Senpai;

            this.FindViewById<TextView>(Resource.Id.UserIdView).Text = lSenpai?.Me?.Id.ToString();
        }

        #endregion
    }
}