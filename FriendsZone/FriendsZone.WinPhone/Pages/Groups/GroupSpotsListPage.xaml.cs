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

namespace FriendsZone.WinPhone.Pages.Groups
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GroupSpotsListPage : Page
    {
        public GroupSpotsListPage()
        {
            this.InitializeComponent();
        }
        string navParam;
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navParam = e.Parameter.ToString();
            getGroupSpots();
        }

        private void processResponse(String json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);
            listViewGroupSpotsList.Items.Clear();
            var spots = JsonConvert.DeserializeObject<List<Spot>>(jsonMsg.msg);
            for (int i = 0; i < spots.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Content = JsonConvert.SerializeObject(spots[i]);
                listViewGroupSpotsList.Items.Add(item);
            }
        }

        private async void getGroupSpots()
        {
            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://friendszone.cba.pl/api/get_group_spots.php?gid={0}",
                    navParam);

            String ResponseString = await httpClient.GetStringAsync(new Uri(url));
            processResponse(ResponseString);
        }

        private void listViewGroupSpotsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            
        }
    }
}
