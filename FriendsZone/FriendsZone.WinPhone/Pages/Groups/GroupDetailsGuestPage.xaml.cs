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
    public sealed partial class GroupDetailsGuestPage : Page
    {
        Group group;

        public GroupDetailsGuestPage()
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
            string groupJson = e.Parameter.ToString();
            group = JsonConvert.DeserializeObject<Group>(groupJson);
            labelName.Text = group.Name;
            labelOpis.Text = group.Description;

            if (!String.IsNullOrWhiteSpace(group.Password))
            {
                buttonJoinGroup.IsEnabled = false;
                textPassword.IsEnabled = true;
            }
        }

        private async void buttonJoinGroup_Click(object sender, RoutedEventArgs e)
        {
            if (textPassword != null)
            {
                if (textPassword.Password != group.Password)
                {
                    var dialog = new MessageDialog("Błędne hasło.");
                    dialog.Title = "Błąd";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    var res = await dialog.ShowAsync();
                    return;
                }
            }

            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/join_group.php?gid={0}&uid={1}",
                group.Id,
                ApplicationData.Current.LocalSettings.Values["Email"]);

            string ResponseString = await httpClient.GetStringAsync(new Uri(url));
            processResponse(ResponseString);
        }

        private async void processResponse(string json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);
            MessageDialog dialog;
            IUICommand res;

            switch (jsonMsg.msg)
            {
                case "error-wrong-values":
                    dialog = new MessageDialog("Błąd danych.");
                    dialog.Title = "Błąd";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                    break;
                case "error-server":
                    dialog = new MessageDialog("Błąd serwera.");
                    dialog.Title = "Błąd";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                    break;
                case "success":
                    dialog = new MessageDialog("Pomyślnie dołączono do grupy.");
                    dialog.Title = "Yay!";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                    Frame.Navigate(typeof(MainPage));
                    break;
            }
        }

        private void textPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(textPassword.Password))
                buttonJoinGroup.IsEnabled = false;
            else
                buttonJoinGroup.IsEnabled = true;
        }
    }
}
