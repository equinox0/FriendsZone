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
    public sealed partial class YourGroupsPage : Page
    {
        public YourGroupsPage()
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
        }

        private async void getUserGroups()
        {
            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/get_user_groups.php?uid={0}",
                    ApplicationData.Current.LocalSettings.Values["Email"]);

            string ResponseString = await httpClient.GetStringAsync(new Uri(url));

            processResponse(ResponseString);
        }

        private async void processResponse(String json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            listViewYourGroupsList.Items.Clear();
            var groups = JsonConvert.DeserializeObject<List<Group>>(jsonMsg.msg);
            for (int i = 0; i < groups.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Content = JsonConvert.SerializeObject(groups[i]);
                listViewYourGroupsList.Items.Add(item);
            }
            var dialog = new MessageDialog("Wczytano Twoje grupy.");
            dialog.Title = "Yay!";
            dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
            var res = await dialog.ShowAsync();
        }

        private void listViewYourGroupsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            ListViewItem item = e.ClickedItem as ListViewItem;
            Frame.Navigate(typeof(GroupDetailsMemberPage), item.Content);
        }
    }
}
