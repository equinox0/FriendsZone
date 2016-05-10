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
        Button buttonLog;
        Button buttonGroups;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            SetContentView(Resource.Layout.Main);

            buttonMap = FindViewById<Button>(Resource.Id.buttonMap);
            buttonLog = FindViewById<Button>(Resource.Id.buttonLog);
            buttonGroups = FindViewById<Button>(Resource.Id.buttonGroups);

            if (!isLogedIn())
            {
                buttonLog.Text = Resources.GetString(Resource.String.btn_logout);
                Intent loginIntent = new Intent(this, typeof(Activities.LoginActivity));
                StartActivity(loginIntent);           
            }

            buttonMap.Click += delegate
            {
                Intent mapIntent = new Intent(this, typeof(Activities.MapActivity));
                StartActivity(mapIntent);
            };


            buttonLog.Click += delegate
            {
                if (buttonLog.Text == Resources.GetString(Resource.String.btn_logout))
                {
                    var prefs = this.GetSharedPreferences("User.data", FileCreationMode.Private);
                    prefs.Edit().Clear().Commit();
                    OnResume();
                }
                else if (buttonLog.Text == Resources.GetString(Resource.String.btn_login))
                {
                    Intent loginIntent = new Intent(this, typeof(Activities.LoginActivity));
                    StartActivity(loginIntent);
                }
            };

            buttonGroups.Click += delegate
            {
                Intent groupsMenuIntent = new Intent(this, typeof(Activities.Groups.GroupsMenuActivity));
                StartActivity(groupsMenuIntent);
            };
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (isLogedIn())
            {
                buttonLog.Text = Resources.GetString(Resource.String.btn_logout);
                buttonMap.Enabled = true;
                buttonGroups.Enabled = true;
                var prefs = this.GetSharedPreferences("User.data", FileCreationMode.Private);
                FindViewById<TextView>(Resource.Id.labelStatus).Text = prefs.GetString("Name", "") + " " + prefs.GetString("Surname", "");
            } else
            {
                buttonLog.Text = Resources.GetString(Resource.String.btn_login);
                buttonMap.Enabled = false;
                buttonGroups.Enabled = false;
                FindViewById<TextView>(Resource.Id.labelStatus).Text = Resources.GetString(Resource.String.not_loged_in);
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


