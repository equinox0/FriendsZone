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

namespace FriendsZone.Droid.Activities
{
    [Activity(Label = "Rejestracja")]
    public class RegisterActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Register);

            Button registerButton = FindViewById<Button>(Resource.Id.buttonRegister);

            registerButton.Click += delegate
            {
                string email = FindViewById<EditText>(Resource.Id.textEmail).Text;
                string name = FindViewById<EditText>(Resource.Id.textName).Text;
                string surname = FindViewById<EditText>(Resource.Id.textSurname).Text;
                string password1 = FindViewById<EditText>(Resource.Id.textPassword1).Text;
                string password2 = FindViewById<EditText>(Resource.Id.textPassword2).Text;

                if (String.IsNullOrWhiteSpace(email) ||
                String.IsNullOrWhiteSpace(password1) ||
                String.IsNullOrWhiteSpace(password2) ||
                String.IsNullOrWhiteSpace(name) ||
                String.IsNullOrWhiteSpace(surname))
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("B³¹d");
                    alert.SetMessage("Wszystkie pola s¹ wymagane.");
                    alert.SetNeutralButton("OK", (senderAlert, args) => {
                        return;
                    });

                    RunOnUiThread(() =>
                    {
                        alert.Show();
                    });

                    return;
                }

                if (password1 != password2)
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("B³¹d");
                    alert.SetMessage("Has³a nie s¹ takie same.");
                    alert.SetNeutralButton("OK", (senderAlert, args) => {
                        return;
                    });

                    RunOnUiThread(() =>
                   {
                       alert.Show();
                   });

                    return;
                }

                string url = string.Format("http://www.friendszone.cba.pl/api/register.php?email={0}&name={1}&surname={2}&password={3}",
                    email,
                    name,
                    surname,
                    password1);

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
            AlertDialog.Builder alert = new AlertDialog.Builder(this);

            switch (jsonMsg.msg)
            {
                case "error-bad-email":
                    alert.SetTitle("B³¹d");
                    alert.SetMessage("B³êdny adres email.");
                    alert.SetNeutralButton("OK", (senderAlert, args) => {
                        return;
                    });

                    RunOnUiThread(() =>
                    {
                        alert.Show();
                    });
                    break;
                case "error-server":
                    alert.SetTitle("B³¹d");
                    alert.SetMessage("B³¹d serwera.");
                    alert.SetNeutralButton("OK", (senderAlert, args) => {
                        return;
                    });

                    RunOnUiThread(() =>
                    {
                        alert.Show();
                    });
                    break;
                case "error-email-exist":
                    alert.SetTitle("B³¹d");
                    alert.SetMessage("Wybrany adres email ju¿ istnieje.");
                    alert.SetNeutralButton("OK", (senderAlert, args) => {
                        return;
                    });

                    RunOnUiThread(() =>
                    {
                        alert.Show();
                    });
                    break;
                case "success":
                    alert.SetTitle("Sukces");
                    alert.SetMessage("Pomyœlnie zarejestrowano. Proszê siê zalogowaæ.");
                    alert.SetNeutralButton("OK", (senderAlert, args) => {
                        return;
                    });

                    RunOnUiThread(() =>
                    {
                        alert.Show();
                    });
                    Finish();
                    break;
            }
        }
    }
}