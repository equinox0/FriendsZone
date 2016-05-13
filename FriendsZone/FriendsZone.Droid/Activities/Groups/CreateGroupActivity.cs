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
using Newtonsoft.Json;
using System.Net;
using System.IO;
using FriendsZone.Helpers;

namespace FriendsZone.Droid.Activities.Groups
{
    [Activity(Label = "Stwórz grupê")]
    public class CreateGroupActivity : Activity
    {
        Spinner spinnerColors;
        Button buttonCreateGroup;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.CreateGroup);

            spinnerColors = FindViewById<Spinner>(Resource.Id.spinnerColors);
            buttonCreateGroup = FindViewById<Button>(Resource.Id.buttonCreateGroup);

            var adapter = ArrayAdapter.CreateFromResource(
                this,
                Resource.Array.colors_array,
                Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerColors.Adapter = adapter;

            buttonCreateGroup.Click += delegate
            {
                string name = FindViewById<EditText>(Resource.Id.textName).Text;
                string description = FindViewById<EditText>(Resource.Id.textDescription).Text;
                string password = FindViewById<EditText>(Resource.Id.textPassword).Text;
                float color = ColorParser.parseColorToFloat(FindViewById<Spinner>(Resource.Id.spinnerColors).SelectedItem.ToString());

                string user = this.GetSharedPreferences("User.data", FileCreationMode.Private).GetString("Email", "");

                if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(description))
                {
                    Toast.MakeText(
                        this,
                        "Brak nazwy lub opisu",
                        ToastLength.Long).Show();

                    return;
                }

                string url = string.Format("http://www.friendszone.cba.pl/api/add_group.php?name={0}&description={1}&password={2}&user={3}&color={4}",
                    name,
                    description,
                    password,
                    user,
                    color
                    );

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
                String json = reader.ReadToEnd();

                processResponse(json);
            };
        }

        private void processResponse(String json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            switch (jsonMsg.msg)
            {
                case "error-server":
                    Toast.MakeText(
                        this,
                        "B³¹d serwera",
                        ToastLength.Long).Show();
                    break;
                case "success":
                    Toast.MakeText(
                        this,
                        "Stworzono",
                        ToastLength.Long).Show();
                    Finish();
                    break;
            }
        }

    }
}