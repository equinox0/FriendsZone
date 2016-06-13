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
    public sealed partial class SearchGroupsPage : Page
    {
        public SearchGroupsPage()
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
        }

        private async void searchGroupsByName(string name)
        {
            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/search_groups.php?name={0}&uid={1}",
                    name,
                    ApplicationData.Current.LocalSettings.Values["Email"]);

            string ResponseString = await httpClient.GetStringAsync(new Uri(url));

            processResponse(ResponseString);
        }

        private void processResponse(string json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            listViewGroupsList.Items.Clear();
            var groups = JsonConvert.DeserializeObject<List<Group>>(jsonMsg.msg);
            for (int i=0; i<groups.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Content = JsonConvert.SerializeObject(groups[i]);
                listViewGroupsList.Items.Add(item);
            }
        }

        private void listViewGroupsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListViewItem item = e.ClickedItem as ListViewItem;
            Frame.Navigate(typeof(GroupDetailsGuestPage), item.Content);
        }

        private void textToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(textToSearch.Text))
            {
                listViewGroupsList.Items.Clear();
                return;
            }
            searchGroupsByName(textToSearch.Text);
        }
    }
}
