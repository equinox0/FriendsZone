using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace FriendsZone.Droid
{
	[Activity (Label = "FriendsZone.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            SetContentView(Resource.Layout.MainMap);

            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate
            {
                var prefs = this.GetSharedPreferences("User.data", FileCreationMode.Private);
                button.Text = isLogedIn().ToString() + prefs.GetString("Email", "");
            };

            //if(!isLogedIn()) {
                Intent loginIntent = new Intent(this, typeof(Activities.LoginActivity));
                StartActivity(loginIntent);
            //}
		}

        private bool isLogedIn()
        {
            var prefs = this.GetSharedPreferences("User.data", FileCreationMode.Private);

            if(prefs.Contains("Email") && prefs.Contains("Name") && prefs.Contains("Surname") && prefs.Contains("Password"))
            {
                return true;
            } else
            {
                return false;
            }
        }
	}
}


