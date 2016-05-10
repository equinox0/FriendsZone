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

namespace FriendsZone.Droid.Activities.Groups
{
    [Activity(Label = "Grupy")]
    public class GroupsMenuActivity : Activity
    {

        Button buttonSearchGroup;
        Button buttonYourGroups;
        Button buttonCreateGroup;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.GroupsMenu);

            buttonSearchGroup = FindViewById<Button>(Resource.Id.buttonSearchGroups);
            buttonYourGroups = FindViewById<Button>(Resource.Id.buttonYourGroups);
            buttonCreateGroup = FindViewById<Button>(Resource.Id.buttonCreateGroup);

            buttonSearchGroup.Click += delegate
            {
                Intent searchGroupIntent = new Intent(this, typeof(SearchGroupsActivity));
                StartActivity(searchGroupIntent);
            };

            buttonYourGroups.Click += delegate
            {
                Intent yourGroupsIntent = new Intent(this, typeof(YourGroupsActivity));
                StartActivity(yourGroupsIntent);
            };

            buttonCreateGroup.Click += delegate
            {
                Intent createGroupIntent = new Intent(this, typeof(CreateGroupActivity));
                StartActivity(createGroupIntent);
            };
        }
    }
}