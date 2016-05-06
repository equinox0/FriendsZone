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

        Button buttonMap;
        Button buttonLogout;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            SetContentView(Resource.Layout.Main);

            buttonMap = FindViewById<Button>(Resource.Id.buttonMap);
            buttonLogout = FindViewById<Button>(Resource.Id.buttonLogout);

            if (!isLogedIn())
            {
                Intent loginIntent = new Intent(this, typeof(Activities.LoginActivity));
                StartActivity(loginIntent);
            }

            buttonMap.Click += delegate
            {
                Intent mapIntent = new Intent(this, typeof(Activities.MapActivity));
                StartActivity(mapIntent);
            };

            buttonLogout.Click += delegate
            {
                var prefs = this.GetSharedPreferences("User.data", FileCreationMode.Private);
                prefs.Edit().Clear().Commit();
                OnResume();
            };
		}

        protected override void OnResume()
        {
            base.OnResume();

            if (isLogedIn())
            {
                buttonMap.Enabled = true;
                buttonLogout.Enabled = true;
                var prefs = this.GetSharedPreferences("User.data", FileCreationMode.Private);
                FindViewById<TextView>(Resource.Id.labelStatus).Text = prefs.GetString("Name", "") + " " + prefs.GetString("Surname", "");
            } else
            {
                buttonMap.Enabled = false;
                buttonLogout.Enabled = false;
                FindViewById<TextView>(Resource.Id.labelStatus).Text = "Nie jesteś zalogowany";
            }
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


