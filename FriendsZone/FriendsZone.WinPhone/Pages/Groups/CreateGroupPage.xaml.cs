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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace FriendsZone.WinPhone.Pages.Groups
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateGroupPage : Page
    {
        public CreateGroupPage()
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

        private async void processResponse(String json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);
            MessageDialog dialog;
            IUICommand res;

            switch (jsonMsg.msg)
            {
                case "error-server":
                    dialog = new MessageDialog("Błąd serwera.");
                    dialog.Title = "Błąd";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                    //if ((int)res.Id == 0)
                    return;
                case "success":
                    dialog = new MessageDialog("Sukces.");
                    dialog.Title = "Yay";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                    //if ((int)res.Id == 0)
                    Frame.Navigate(typeof(GroupMenuPage));
                    return;
            }
        }

        private async void buttonCreateGroup_Click(object sender, RoutedEventArgs e)
        {
            string name = textName.Text;
            string description = textDescription.Text;
            string password = textPassword.Password;
            float color = ColorParser.parseColorToFloat(comboBoxColors.SelectedItem.ToString());

            string user = ApplicationData.Current.LocalSettings.Values["Email"].ToString();

            if (String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(description))
            {
                var dialog = new MessageDialog("Brak nazwy lub opisu.");
                dialog.Title = "Błąd";
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                var res = await dialog.ShowAsync();
                return;
            }

            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/add_group.php?name={0}&description={1}&password={2}&user={3}&color={4}",
                name,
                description,
                password,
                user,
                color
                );            

            string ResponseString = await httpClient.GetStringAsync(new Uri(url));

            processResponse(ResponseString);
        }
    }
}
