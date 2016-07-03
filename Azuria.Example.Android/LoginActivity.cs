using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace Azuria.Example.Android
{
    [Activity(Label = "Azuria.Example.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class LoginActivity : Activity
    {
        #region

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            // Set our view from the "main" layout resource
            this.SetContentView(Resource.Layout.Login);

            Button lLoginButton = this.FindViewById<Button>(Resource.Id.LoginButton);

            lLoginButton.Click += async (sender, args) =>
            {
                string lUsername = this.FindViewById<EditText>(Resource.Id.UsernameBox).Text;
                string lPassword = this.FindViewById<EditText>(Resource.Id.PasswordBox).Text;
                Senpai lSenpai = new Senpai();
                await lSenpai.Login(lUsername, lPassword);

                Intent lMainActivity = new Intent(this, typeof(MainActivity));
                lMainActivity.PutExtra("SenpaiParcelable", new SenpaiParcelable(lSenpai));
                this.StartActivity(lMainActivity);
            };
        }

        #endregion
    }
}