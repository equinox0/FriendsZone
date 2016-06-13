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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace FriendsZone.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
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

        private async void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();                

            string url = string.Format("http://www.friendszone.cba.pl/api/login.php?email={0}&password={1}",
                textEmail.Text,
                textPassword.Password);
            
            string ResponseString = await httpClient.GetStringAsync(new Uri(url));

            processResponse(ResponseString);
        }

        private void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }

        private async void processResponse(String json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);

            switch (jsonMsg.msg)
            {
                case "error-login":                    
                    var dialog = new MessageDialog("Zły login lub hasło.");
                    dialog.Title = "Błąd";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    var res = await dialog.ShowAsync();
                    //if ((int)res.Id == 0)
                    break;
                default:
                    Models.Users user = JsonConvert.DeserializeObject<Models.Users>(jsonMsg.msg);
                    loginUser(user);
                    Frame.Navigate(typeof(MainPage));
                    break;
            }
        }

        private void loginUser(Models.Users user)
        {
            var d = ApplicationData.Current.LocalSettings;
            d.Values["Email"] = user.email;
            d.Values["Name"] = user.name;
            d.Values["Surname"] = user.surname;
            d.Values["Password"] = user.password;
        }
    }
}
