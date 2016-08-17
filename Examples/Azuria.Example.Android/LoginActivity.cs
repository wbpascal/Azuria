using System.Threading.Tasks;
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
            this.SetContentView(Resource.Layout.Login);

            Button lLoginButton = this.FindViewById<Button>(Resource.Id.LoginButton);

            lLoginButton.Click += (sender, args) =>
            {
                string lUsername = this.FindViewById<EditText>(Resource.Id.UsernameBox).Text;
                string lPassword = this.FindViewById<EditText>(Resource.Id.PasswordBox).Text;
                Senpai lSenpai = new Senpai();
                var lResult = Task.Run(() => lSenpai.Login(lUsername, lPassword)).Result;

                Intent lMainActivity = new Intent(this, typeof(MainActivity));
                lMainActivity.PutExtra("SenpaiParcelable", new SenpaiParcelable(lSenpai));
                this.StartActivity(lMainActivity);
            };
        }

        #endregion
    }
}