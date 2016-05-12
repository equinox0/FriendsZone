

using Android.App;
using Android.OS;
using Android.Widget;
using FriendsZone.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Text;
using static Android.Widget.AdapterView;
using Android.Content;

namespace FriendsZone.Droid.Activities.Groups
{
    [Activity(Label = "Szukaj grup")]
    public class SearchGroupsActivity : Activity
    {
        EditText textToSearch;
        ListView listViewFoundGroups;

        ArrayAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SearchGroup);

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1);

            listViewFoundGroups = FindViewById<ListView>(Resource.Id.listViewGroupList);

            listViewFoundGroups.Adapter = adapter;

            listViewFoundGroups.ItemClick += (object sender, ItemClickEventArgs e) =>
            {
                string selectedJson = JsonConvert.SerializeObject(listViewFoundGroups.GetItemAtPosition(e.Position).Cast<Group>());
                Intent detailIntent = new Intent(this, typeof(GroupDetailsGuestActivity));
                detailIntent.PutExtra("GROUP_JSON", selectedJson);
                StartActivity(detailIntent);
            };

            textToSearch = FindViewById<EditText>(Resource.Id.textGroupName);

            textToSearch.TextChanged += delegate
            {
                if (String.IsNullOrWhiteSpace(textToSearch.Text))
                {
                    adapter.Clear();
                    return;
                }
                searchGroupsByName(textToSearch.Text);
            };
        }

        protected override void OnResume()
        {
            base.OnResume();
            textToSearch.Text = "";
            adapter.Clear();
        }

        private void searchGroupsByName(string name)
        {
            string url = string.Format("http://www.friendszone.cba.pl/api/search_groups.php?name={0}&uid={1}",
                    name,
                    this.GetSharedPreferences("User.data", FileCreationMode.Private).GetString("Email", ""));

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
            string json = reader.ReadToEnd();

            processResponse(json);
        }

        private void processResponse(string json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            adapter.Clear();

            adapter.AddAll(JsonConvert.DeserializeObject<List<Group>>(jsonMsg.msg));
        }
    }

    public static class ObjectTypeHelper
    {
        public static T Cast<T>(this Java.Lang.Object obj) where T : class
        {
            var propertyInfo = obj.GetType().GetProperty("Instance");
            return propertyInfo == null ? null : propertyInfo.GetValue(obj, null) as T;
        }
    }
}