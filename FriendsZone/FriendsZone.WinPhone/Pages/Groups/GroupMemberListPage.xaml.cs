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
    public sealed partial class GroupMemberListPage : Page
    {
        String navParam;
        public GroupMemberListPage()
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
            navParam = e.Parameter.ToString();
            getGroupMembers();
        }

        private async void getGroupMembers()
        {
            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/get_group_members.php?gid={0}",
                    navParam);

            string ResponseString = await httpClient.GetStringAsync(new Uri(url));
            processResponse(ResponseString);
        }

        private void processResponse(String json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            listViewGroupMembersList.Items.Clear();
            var users = JsonConvert.DeserializeObject<List<Users>>(jsonMsg.msg);
            for (int i = 0; i < users.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Content = JsonConvert.SerializeObject(users[i]);
                listViewGroupMembersList.Items.Add(item);
            }
        }
    }
}
