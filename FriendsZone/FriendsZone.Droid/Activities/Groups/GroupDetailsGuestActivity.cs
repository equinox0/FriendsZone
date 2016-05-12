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
using FriendsZone.Models;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace FriendsZone.Droid.Activities.Groups
{
    [Activity(Label = "GroupDetailsGuestActivity")]
    public class GroupDetailsGuestActivity : Activity
    {
        TextView labelName;
        TextView labelDescription;
        Button buttonJoinGroup;

        Group group;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.GroupDetailsGuest);

            labelName = FindViewById<TextView>(Resource.Id.labelGroupName);
            labelDescription = FindViewById<TextView>(Resource.Id.labelGroupDescription);
            buttonJoinGroup = FindViewById<Button>(Resource.Id.buttonJoinGroup);

            labelName.Visibility = ViewStates.Visible;
            labelDescription.Visibility = ViewStates.Visible;

            string groupJson = Intent.GetStringExtra("GROUP_JSON");

            group = JsonConvert.DeserializeObject<Group>(groupJson);
            labelName.Text = group.Name;
            labelDescription.Text = group.Description;

            buttonJoinGroup.Click += delegate
            {
                string url = string.Format("http://www.friendszone.cba.pl/api/join_group.php?gid={0}&uid={1}",
                    group.Id,
                    this.GetSharedPreferences("User.data", FileCreationMode.Private).GetString("Email", ""));

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
                string json = reader.ReadToEnd();

                processResponse(json);
            };
        }

        private void processResponse(string json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            switch(jsonMsg.msg)
            {
                case "error-wrong-values":
                    Toast.MakeText(
                        this,
                        "B³¹d danych",
                        ToastLength.Long).Show();
                    break;
                case "error-server":
                    Toast.MakeText(
                        this,
                        "B³¹d serwera",
                        ToastLength.Long).Show();
                    break;
                case "success":
                    Toast.MakeText(
                        this,
                        "Pomyœlnie do³¹czono do grupy",
                        ToastLength.Long).Show();
                    Finish();
                    break;
            }
        }
    }
}