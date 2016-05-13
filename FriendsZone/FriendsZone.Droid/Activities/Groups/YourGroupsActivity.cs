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
using static Android.Widget.AdapterView;

namespace FriendsZone.Droid.Activities.Groups
{
    [Activity(Label = "Twoje grupy")]
    public class YourGroupsActivity : Activity
    {
        ListView listViewYourGroups;

        ArrayAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ItemList);

            listViewYourGroups = FindViewById<ListView>(Resource.Id.listViewItemList);

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1);

            listViewYourGroups.Adapter = adapter;

            getUserGroups();

            listViewYourGroups.ItemClick += (object sender, ItemClickEventArgs e) =>
            {
                string selectedJson = JsonConvert.SerializeObject(listViewYourGroups.GetItemAtPosition(e.Position).Cast<Group>());
                Intent detailIntent = new Intent(this, typeof(GroupDetailsMemberActivity));
                detailIntent.PutExtra("GROUP_JSON", selectedJson);
                StartActivity(detailIntent);
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            getUserGroups();
        }

        private void getUserGroups()
        {
            string url = string.Format("http://www.friendszone.cba.pl/api/get_user_groups.php?uid={0}",
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

            adapter.Clear();
            adapter.AddAll(JsonConvert.DeserializeObject<List<Group>>(jsonMsg.msg));
            Toast.MakeText(
                this,
                "Wczytano twoje grupy",
                ToastLength.Long).Show();
        }
    }
}