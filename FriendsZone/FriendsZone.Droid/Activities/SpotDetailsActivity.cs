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
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace FriendsZone.Droid.Activities
{
    [Activity(Label = "SpotDetailsActivity")]
    public class SpotDetailsActivity : Activity
    {
        int groupId;
        List<Group> userGroups;

        Spinner spinnerGroups;
        Button buttonSave;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SpotDetails);

            //ustawia zawartoœæ spinnera
            spinnerGroups = FindViewById<Spinner>(Resource.Id.spinnerGroups);
            getUserGroups();
            var adapter = new ArrayAdapter<Group>(
                this,
                Android.Resource.Layout.SimpleSpinnerItem,
                userGroups
                );
            
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerGroups.Adapter = adapter;

            //sprawdza czy uruchomiono z grupy
            groupId = Intent.GetIntExtra("GROUP_ID", -1);

            if (groupId != -1)
            {
                Group currentSelectedGroup = new Group();
                foreach (Group g in userGroups)
                {
                    if (g.Id == groupId) currentSelectedGroup = g;
                }
                spinnerGroups.SetSelection(adapter.GetPosition(currentSelectedGroup));
                spinnerGroups.Enabled = false;
            }

            buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
            buttonSave.Click += delegate
            {
                string name = FindViewById<EditText>(Resource.Id.textName).Text;
                string des = FindViewById<EditText>(Resource.Id.textDescription).Text;
                int groupId = adapter.GetItem(spinnerGroups.SelectedItemPosition).Id;
                double lat = Intent.GetDoubleExtra("LAT", -1);
                double lon = Intent.GetDoubleExtra("LONG", -1);
                if(String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(des) || lat == -1 || lon == -1)
                {
                    Toast.MakeText(
                        this,
                        "B³¹d przy dodawaniu miejsca",
                        ToastLength.Long).Show();
                    return;
                }

                string url = string.Format("http://www.friendszone.cba.pl/api/add_spot.php?name={0}&des={1}&lat={2}&lon={3}&gid={4}",
                    name,
                    des,
                    lat,
                    lon,
                    groupId);

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
                String json = reader.ReadToEnd();

                Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

                if(jsonMsg.msg == "success")
                {
                    Toast.MakeText(
                        this,
                        "Pomyœlnie dodano nowe miejsce",
                        ToastLength.Long).Show();
                    Finish();
                }
                else if (jsonMsg.msg == "error-server")
                {
                    Toast.MakeText(
                        this,
                        "B³¹d serwera",
                        ToastLength.Long).Show();
                    return;
                }
            };
        }

        // jeœli nie ma podanej grupy, pobierze wszystkie
        private void getUserGroups()
        {
            string url = string.Format("http://www.friendszone.cba.pl/api/get_user_groups.php?uid={0}",
                    this.GetSharedPreferences("User.data", FileCreationMode.Private).GetString("Email", ""));

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
            String json = reader.ReadToEnd();

            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            userGroups = new List<Group>();
            foreach (Group g in JsonConvert.DeserializeObject<List<Group>>(jsonMsg.msg))
            {
                userGroups.Add(g);
            }
        }
    }
}