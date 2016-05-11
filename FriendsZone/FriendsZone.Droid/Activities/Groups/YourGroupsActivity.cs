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
    [Activity(Label = "Twoje grupy")]
    public class YourGroupsActivity : Activity
    {
        ListView listViewYourGroups;
        List<Group> data;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.YourGroups);

            listViewYourGroups = FindViewById<ListView>(Resource.Id.listViewYouGroups);

            getUserGroups();

            listViewYourGroups.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, data);
        }

        private void getUserGroups()
        {
            string url = string.Format("http://www.friendszone.cba.pl/api/get_user_groups.php?user={0}",
                    this.GetSharedPreferences("User.data", FileCreationMode.Private).GetString("Email", ""));

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
            String json = reader.ReadToEnd();

            processResponse(json);
        }

        private void processResponse(String json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            data = JsonConvert.DeserializeObject<List<Group>>(jsonMsg.msg);
            Toast.MakeText(
                this,
                "Wczytano twoje grupy",
                ToastLength.Long).Show();
        }
    }
}