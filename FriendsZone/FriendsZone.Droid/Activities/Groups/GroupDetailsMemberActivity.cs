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
using FriendsZone.Helpers;

namespace FriendsZone.Droid.Activities.Groups
{
    [Activity(Label = "GroupDetailsMemberActivity")]
    public class GroupDetailsMemberActivity : Activity
    {
        Group group;
        bool isOwner;
        string baseColor, baseName, baseDescription, basePassword; 

        // Member
        TextView labelName, labelDescription;

        // Owner
        EditText textName, textDescription, textPassword;

        // All
        Spinner spinnerColors;
        Button buttonSave, buttonSpots, buttonMembers, buttonLeaveGroup;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.GroupDetailsMember);

            string groupJson = Intent.GetStringExtra("GROUP_JSON");
            group = JsonConvert.DeserializeObject<Group>(groupJson);

            isOwner = setIsOwner();

            buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
            buttonSpots = FindViewById<Button>(Resource.Id.buttonSpotList);
            buttonMembers = FindViewById<Button>(Resource.Id.buttonMemberList);
            buttonLeaveGroup = FindViewById<Button>(Resource.Id.buttonLeaveGroup);

            if (isOwner)
            {
                textName = FindViewById<EditText>(Resource.Id.textGroupName);
                textDescription = FindViewById<EditText>(Resource.Id.textGroupDescription);
                textPassword = FindViewById<EditText>(Resource.Id.textPassword);

                textName.Visibility = ViewStates.Visible;
                textDescription.Visibility = ViewStates.Visible;
                textPassword.Visibility = ViewStates.Visible;

                baseName = group.Name;
                baseDescription = group.Description;
                basePassword = group.Password;

                textName.Text = baseName;
                textDescription.Text = baseDescription;
                textPassword.Text = basePassword;

                textName.AfterTextChanged += delegate
                {
                    checkChanges();
                };

                textDescription.AfterTextChanged += delegate
                {
                    checkChanges();
                };

                textPassword.AfterTextChanged += delegate
                {
                    checkChanges();
                };

                buttonLeaveGroup.Text = "Usuñ grupê";
            } else
            {
                labelName = FindViewById<TextView>(Resource.Id.labelGroupName);
                labelDescription = FindViewById<TextView>(Resource.Id.labelGroupDescription);

                labelName.Visibility = ViewStates.Visible;
                labelDescription.Visibility = ViewStates.Visible;
            }

            spinnerColors = FindViewById<Spinner>(Resource.Id.spinnerColors);

            baseColor = setColor();

            var adapter = ArrayAdapter.CreateFromResource(
                this,
                Resource.Array.colors_array,
                Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerColors.Adapter = adapter;

            spinnerColors.SetSelection(adapter.GetPosition(baseColor));

            spinnerColors.ItemSelected += delegate
            {
                checkChanges();
            };

            buttonSave.Click += delegate
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetTitle("Uwaga!");
                builder.SetMessage("Czy na pewno chcesz zapisaæ dane?");
                builder.SetCancelable(false);
                builder.SetPositiveButton("OK", delegate
                    {
                        bool colorEditSuccess = true;
                        bool groupEditSuccess = true;

                        if (spinnerColors.SelectedItem.ToString() != baseColor)
                        {
                            string url = string.Format("http://friendszone.cba.pl/api/edit_group_member.php?gid={0}&uid={1}&color={2}",
                                    group.Id,
                                    this.GetSharedPreferences("User.data", FileCreationMode.Private).GetString("Email", ""),
                                    ColorParser.parseColorToFloat(spinnerColors.SelectedItem.ToString()));

                            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                            request.Method = "GET";

                            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                            StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
                            String json = reader.ReadToEnd();

                            if(JsonConvert.DeserializeObject<Helpers.JsonMsg>(json).msg != "success")
                            {
                                colorEditSuccess = false;
                            }
                        }

                        if (isOwner && (textName.Text != baseName || textDescription.Text != baseDescription || textPassword.Text != basePassword))
                        {
                            string url = string.Format("http://friendszone.cba.pl/api/edit_group.php?gid={0}&name={1}&des={2}&pass={3}",
                                group.Id,
                                textName.Text,
                                textDescription.Text,
                                textPassword.Text);

                            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                            request.Method = "GET";

                            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                            StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
                            String json = reader.ReadToEnd();

                            if (JsonConvert.DeserializeObject<Helpers.JsonMsg>(json).msg != "success")
                            {
                                groupEditSuccess = false;
                            }
                        }

                        if (colorEditSuccess && groupEditSuccess)
                        {
                            Toast.MakeText(
                                this,
                                "Pomyœlnie edytowano grupê",
                                ToastLength.Long).Show();
                            Finish();
                        } else
                        {
                            Toast.MakeText(
                                   this,
                                   "Wyst¹pi³ b³¹d w trakcie edycji",
                                   ToastLength.Long).Show();
                        }
                    });
                builder.SetNegativeButton("Anuluj", delegate { return; });
                builder.Show();
            };

            buttonMembers.Click += delegate
            {
                Intent memberListIntent = new Intent(this, typeof(GroupMemberListActivity));
                memberListIntent.PutExtra("GROUP_ID", group.Id);
                StartActivity(memberListIntent);
            };

            buttonSpots.Click += delegate
            {
                Intent spotsListIntent = new Intent(this, typeof(GroupSpotsListActivity));
                spotsListIntent.PutExtra("GROUP_ID", group.Id);
                StartActivity(spotsListIntent);
            };

            buttonLeaveGroup.Click += delegate
            {
                if(isOwner)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetTitle("Uwaga!");
                        builder.SetMessage("Usuniêcie grupy jest nieodwracalne.");
                        builder.SetCancelable(false);
                        builder.SetPositiveButton("OK", delegate {
                            string url = string.Format("http://friendszone.cba.pl/api/delete_group.php?gid={0}",
                                group.Id);

                            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                            request.Method = "GET";

                            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                            StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
                            String json = reader.ReadToEnd();

                            processLeaveResponse(json);
                            // success, error-server
                        });
                        builder.SetNegativeButton("Anuluj", delegate { return; });
                        builder.Show();     
                }
                else
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Uwaga!");
                    builder.SetMessage("Czy na pewno chcesz opuœciæ grupê?");
                    builder.SetCancelable(false);
                    builder.SetPositiveButton("OK", delegate {
                        string url = string.Format("http://friendszone.cba.pl/api/leave_group.php?gid={0}&uid={1}",
                        group.Id,
                        this.GetSharedPreferences("User.data", FileCreationMode.Private).GetString("Email", ""));

                        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                        request.Method = "GET";

                        HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                        StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
                        String json = reader.ReadToEnd();

                        processLeaveResponse(json);
                        // success, error-server
                    });
                    builder.SetNegativeButton("Anuluj", delegate { return; });
                    builder.Show();
                }
            };
        }

        private void processLeaveResponse(string json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            switch(jsonMsg.msg)
            {
                case "success":
                    if(isOwner)
                    {
                        Toast.MakeText(
                        this,
                        "Usuniêto grupê",
                        ToastLength.Long).Show();
                    } else
                    {
                        Toast.MakeText(
                        this,
                        "Opuszczono grupê",
                        ToastLength.Long).Show();
                    }
                    Finish();
                    break;
                case "error-server":
                    Toast.MakeText(
                        this,
                        "Wczytano twoje grupy",
                        ToastLength.Long).Show();
                    break;
            }
        }

        private void checkChanges()
        {
            if(isOwner)
            {
                if (spinnerColors.SelectedItem.ToString() != baseColor || textName.Text != baseName || textDescription.Text != baseDescription || textPassword.Text != basePassword)
                {
                    buttonSave.Enabled = true;
                }
                else
                {
                    buttonSave.Enabled = false;
                }
            } else
            {
                if (spinnerColors.SelectedItem.ToString() != baseColor)
                {
                    buttonSave.Enabled = true;
                }
                else
                {
                    buttonSave.Enabled = false;
                }
            }
            
        }

        private bool setIsOwner()
        {
            string url = string.Format("http://www.friendszone.cba.pl/api/get_group_status.php?gid={0}&uid={1}",
                    group.Id,
                    this.GetSharedPreferences("User.data", FileCreationMode.Private).GetString("Email", ""));

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
            string json = reader.ReadToEnd();
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            if (jsonMsg.msg == "owner")
            {
                return true;
            } else
            {
                return false;
            }
        }

        private string setColor()
        {
            string url = string.Format("http://www.friendszone.cba.pl/api/get_group_color.php?gid={0}&uid={1}",
                    group.Id,
                    this.GetSharedPreferences("User.data", FileCreationMode.Private).GetString("Email", ""));

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            StreamReader reader = new StreamReader(response.GetResponseStream(), UTF8Encoding.UTF8);
            string json = reader.ReadToEnd();
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            return ColorParser.parseFloatToColor(float.Parse(jsonMsg.msg));
        }
        
    }
}