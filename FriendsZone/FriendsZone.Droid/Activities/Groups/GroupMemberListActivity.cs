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
using System.Net;
using System.IO;
using Newtonsoft.Json;
using FriendsZone.Models;
using static Android.Widget.AdapterView;

namespace FriendsZone.Droid.Activities.Groups
{
    [Activity(Label = "Lista u¿ytkowników")]
    public class GroupMemberListActivity : Activity
    {
        ListView listViewGroupMembers;

        ArrayAdapter adapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ItemList);

            listViewGroupMembers = FindViewById<ListView>(Resource.Id.listViewItemList);

            adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1);

            listViewGroupMembers.Adapter = adapter;

            getGroupMembers();
        }

        private void getGroupMembers()
        { 
            string url = string.Format("http://www.friendszone.cba.pl/api/get_group_members.php?gid={0}",
                    Intent.GetIntExtra("GROUP_ID", 0));

            Console.WriteLine(url);

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
            String json = reader.ReadToEnd();

            processResponse(json);
        }

        private void processResponse(String json)
        {
            Console.WriteLine(json);
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            adapter.Clear();
            adapter.AddAll(JsonConvert.DeserializeObject<List<Users>>(jsonMsg.msg));
            Toast.MakeText(
                this,
                "Wczytano listê cz³onków grupy",
                ToastLength.Long).Show();
        }
    }
}