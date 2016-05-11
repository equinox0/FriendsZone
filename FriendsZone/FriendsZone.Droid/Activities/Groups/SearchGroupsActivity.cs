

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using FriendsZone.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace FriendsZone.Droid.Activities.Groups
{
    [Activity(Label = "Szukaj grup")]
    public class SearchGroupsActivity : Activity
    {
        EditText textToSearch;
        ListView listViewFoundGroups;
        List<Group> data;

        ArrayAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SearchGroup);

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1);

            listViewFoundGroups = FindViewById<ListView>(Resource.Id.listViewGroupList);

            listViewFoundGroups.Adapter = adapter;

            textToSearch = FindViewById<EditText>(Resource.Id.textGroupName);

            textToSearch.TextChanged += delegate
            {
                if (String.IsNullOrWhiteSpace(textToSearch.Text)) return; 
                searchGroupsByName(textToSearch.Text);
            };
        }


        private void searchGroupsByName(string name)
        {
            string url = string.Format("http://www.friendszone.cba.pl/api/search_groups.php?name={0}",
                    name);

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
}