using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Newtonsoft.Json;
using Windows.UI.Popups;
using Windows.Storage;
using FriendsZone.Helpers;
using FriendsZone.WinPhone.Pages.Groups;
using FriendsZone.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace FriendsZone.WinPhone.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SpotDetails : Page
    {
        int groupId;
        int spotId;
        Spot spot;
        List<Group> userGroups;

        public SpotDetails()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            getUserGroups();
            foreach (Group g in userGroups)
                comboBoxGroups.Items.Add(JsonConvert.SerializeObject(g));

            groupId = int.Parse(e.Parameter.ToString());
        }

        private async void getSpotById(int spotId)
        {
            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/get_spot.php?sid={0}",
                        spotId);

            String ResponseString = await httpClient.GetStringAsync(new Uri(url));

            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(ResponseString);

            spot = JsonConvert.DeserializeObject<Spot>(jsonMsg.msg);
        }

        private async void getUserGroups()
        {
            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/get_user_groups.php?uid={0}",
                    ApplicationData.Current.LocalSettings.Values["Email"]);

            String ResponseString = await httpClient.GetStringAsync(new Uri(url));

            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(ResponseString);

            userGroups = new List<Group>();
            foreach (Group g in JsonConvert.DeserializeObject<List<Group>>(jsonMsg.msg))
            {
                userGroups.Add(g);
            }
        }
    }
}
