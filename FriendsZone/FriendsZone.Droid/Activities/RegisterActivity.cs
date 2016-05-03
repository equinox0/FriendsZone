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
            };
        }
    }
}