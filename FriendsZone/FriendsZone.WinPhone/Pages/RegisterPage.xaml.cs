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
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
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

        private async void buttonRegister_Click(object sender, RoutedEventArgs e)
        {
            string email = textEmail.Text;
            string name = textName.Text;
            string surname = textSurname.Text;
            string password1 = textPassword1.Password;
            string password2 = textPassword2.Password;

            if (String.IsNullOrWhiteSpace(email) ||
            String.IsNullOrWhiteSpace(password1) ||
            String.IsNullOrWhiteSpace(password2) ||
            String.IsNullOrWhiteSpace(name) ||
            String.IsNullOrWhiteSpace(surname))
            {
                var dialog = new MessageDialog("Wszystkie pola są wymagane.");
                dialog.Title = "Błąd";
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                var res = await dialog.ShowAsync();
                //if ((int)res.Id == 0)
                return;
            }

            if (password1 != password2)
            {
                var dialog = new MessageDialog("Hasła nie są takie same.");
                dialog.Title = "Błąd";
                dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                var res = await dialog.ShowAsync();
                //if ((int)res.Id == 0)
                return;
            }

            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/register.php?email={0}&name={1}&surname={2}&password={3}",
                email,
                name,
                surname,
                password1);

            string ResponseString = await httpClient.GetStringAsync(new Uri(url));

            processResponse(ResponseString);
        }

        private async void processResponse(String json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);
            MessageDialog dialog;
            IUICommand res;

            switch (jsonMsg.msg)
            {
                case "error-bad-email":
                    dialog = new MessageDialog("Błędny adres email.");
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
                case "error-email-exist":
                    dialog = new MessageDialog("Wybrany adres email już istnieje.");
                    dialog.Title = "Błąd";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                    break;
                case "success":
                    dialog = new MessageDialog("Rejestracja pomyślna. Proszę się zalogować.");
                    dialog.Title = "Błąd";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                    Frame.Navigate(typeof(LoginPage));
                    break;
            }
        }
    }
}
