
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using System.IO;
using System.Net;
using System.Text;

using FriendsZone;
using Newtonsoft.Json;

namespace FriendsZone.Droid.Activities
{
    [Activity(Label = "Zaloguj siê")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Login);

            Button buttonLogin = FindViewById<Button>(Resource.Id.buttonLogin);
            Button buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);

            buttonLogin.Click += delegate
            {
                string url = string.Format("http://www.friendszone.cba.pl/api/login.php?email={0}&password={1}", 
                    FindViewById<EditText>(Resource.Id.textEmail).Text, 
                    FindViewById<EditText>(Resource.Id.textPassword).Text);

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
                String json = reader.ReadToEnd();

                processResponse(json);
            };

            buttonRegister.Click += delegate
            {
                Intent registerIntent = new Intent(this, typeof(Activities.RegisterActivity));
                StartActivity(registerIntent);
            };
        }

        private void processResponse(String json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            switch (jsonMsg.msg)
            {
                case "error-login":
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("B³¹d");
                    alert.SetMessage("Z³y login lub has³o.");
                    alert.SetNeutralButton("OK", (senderAlert, args) => {
                        return;
                    });

                    RunOnUiThread(() =>
                    {
                        alert.Show();
                    });
                    break;
                default:
                    Common.Users user = JsonConvert.DeserializeObject<Common.Users>(jsonMsg.msg);
                    loginUser(user);
                    Finish();
                    break;
            }
        }

        private void loginUser(Common.Users user)
        {
            var prefs = this.GetSharedPreferences("User.data", FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutString("Email", user.email);
            editor.PutString("Name", user.name);
            editor.PutString("Surname", user.surname);
            editor.PutString("Password", user.password);
            editor.Commit();
        }
    }
}