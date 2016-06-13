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
    public sealed partial class GroupDetailsMemberPage : Page
    {
        Group group;
        bool isOwner;
        string baseColor, baseName, baseDescription, basePassword;

        public GroupDetailsMemberPage()
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
            group = JsonConvert.DeserializeObject<Group>(e.Parameter.ToString());
            setIsOwner();

            if (isOwner)
            {
                textGroupName.Visibility = Visibility.Visible;
                textGroupDescription.Visibility = Visibility.Visible;
                passwordBox.Visibility = Visibility.Visible;

                baseName = group.Name;
                baseDescription = group.Description;
                basePassword = group.Password;

                textGroupName.Text = baseName;
                textGroupDescription.Text = baseDescription;
                passwordBox.Password = basePassword;

                buttonLeaveGroup.Content = "Usuń grupę";
            }
            else
            {
                /* nie ma labeli dla nieownera
                labelGroupName = FindViewById<TextView>(Resource.Id.labelGroupName);
                labelDescription = FindViewById<TextView>(Resource.Id.labelGroupDescription);

                labelName.Visibility = ViewStates.Visible;
                labelDescription.Visibility = ViewStates.Visible;
                */
            }

            setColor();
        }

        private async void processLeaveResponse(string json)
        {
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(json);
            MessageDialog dialog;
            IUICommand res;

            switch (jsonMsg.msg)
            {
                case "success":
                    if (isOwner)
                    {
                        dialog = new MessageDialog("Usunięto grupę.");
                        dialog.Title = "Yay!";
                        dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                        res = await dialog.ShowAsync();
                    }
                    else
                    {
                        dialog = new MessageDialog("Opuszczono grupę.");
                        dialog.Title = "Yay!";
                        dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                        res = await dialog.ShowAsync();
                    }
                    Frame.Navigate(typeof(GroupMenuPage));
                    break;
                case "error-server":
                    dialog = new MessageDialog("Błąd serwera.");
                    dialog.Title = "Błąd";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                    break;
            }
        }

        private async void buttonLeaveGroup_Click(object sender, RoutedEventArgs e)
        {
            if (isOwner)
            {
                var dialog = new MessageDialog("Usunięcie grupy jest nieodwracalne.");
                dialog.Title = "Uwaga";
                dialog.Commands.Add(new UICommand { Label = "Usuń", Id = 0 });
                dialog.Commands.Add(new UICommand { Label = "Anuluj", Id = 1 });
                var res = await dialog.ShowAsync();
                if ((int)res.Id == 0)
                {
                    HttpClient httpClient = new HttpClient();
                    string url = string.Format("http://friendszone.cba.pl/api/delete_group.php?gid={0}",
                        group.Id);

                    string ResponseString = await httpClient.GetStringAsync(new Uri(url));
                    processLeaveResponse(ResponseString);
                }
            }
            else
            {
                var dialog = new MessageDialog("Czy na pewno chcesz opuścić grupę?");
                dialog.Title = "Uwaga";
                dialog.Commands.Add(new UICommand { Label = "Opuść", Id = 0 });
                dialog.Commands.Add(new UICommand { Label = "Anuluj", Id = 1 });
                var res = await dialog.ShowAsync();
                if ((int)res.Id == 0)
                {
                    HttpClient httpClient = new HttpClient();
                    string url = string.Format("http://friendszone.cba.pl/api/leave_group.php?gid={0}&uid={1}",
                    group.Id,
                    ApplicationData.Current.LocalSettings.Values["Email"]);

                    string ResponseString = await httpClient.GetStringAsync(new Uri(url));
                    processLeaveResponse(ResponseString);
                }
            }
        }

        private void buttonSpotList_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupSpotsListPage), group.Id);
        }

        private void buttonMemberList_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupMemberListPage), group.Id);
        }

        private async void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Czy na pewno chcesz zapisać dane?");
            dialog.Title = "Uwaga";
            dialog.Commands.Add(new UICommand { Label = "Zapisz", Id = 0 });
            dialog.Commands.Add(new UICommand { Label = "Anuluj", Id = 1 });
            var res = await dialog.ShowAsync();
            if ((int)res.Id == 0)
            {
                bool colorEditSuccess = true;
                bool groupEditSuccess = true;
                HttpClient httpClient;
                string ResponseString;
                if (comboBoxColors.SelectedItem.ToString() != baseColor)
                {
                    httpClient = new HttpClient();
                    string url = string.Format("http://friendszone.cba.pl/api/edit_group_member.php?gid={0}&uid={1}&color={2}",
                            group.Id,
                            ApplicationData.Current.LocalSettings.Values["Email"],
                    ColorParser.parseColorToFloat(comboBoxColors.SelectedItem.ToString()));

                    ResponseString = await httpClient.GetStringAsync(new Uri(url));

                    if (JsonConvert.DeserializeObject<Helpers.JsonMsg>(ResponseString).msg != "success")
                    {
                        colorEditSuccess = false;
                    }
                }

                if (isOwner && (textGroupName.Text != baseName || textGroupDescription.Text != baseDescription || passwordBox.Password != basePassword))
                {
                    httpClient = new HttpClient();
                    string url = string.Format("http://friendszone.cba.pl/api/edit_group.php?gid={0}&name={1}&des={2}&pass={3}",
                        group.Id,
                        textGroupName.Text,
                        textGroupDescription.Text,
                        passwordBox.Password);

                    ResponseString = await httpClient.GetStringAsync(new Uri(url));

                    if (JsonConvert.DeserializeObject<Helpers.JsonMsg>(ResponseString).msg != "success")
                    {
                        groupEditSuccess = false;
                    }
                }

                if (colorEditSuccess && groupEditSuccess)
                {
                    dialog = new MessageDialog("Pomyślnie zedytowano dane.");
                    dialog.Title = "Yay!";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                }
                else
                {
                    dialog = new MessageDialog("Wystąpił błąd w czasie edycji.");
                    dialog.Title = "Uwaga";
                    dialog.Commands.Add(new UICommand { Label = "OK", Id = 0 });
                    res = await dialog.ShowAsync();
                }
            }
        }

        private void comboBoxColors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkChanges();
        }

        private void textGroupName_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkChanges();
        }

        private void textGroupDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkChanges();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            checkChanges();
        }

        private void checkChanges()
        {
            if (isOwner)
            {
                if (comboBoxColors.SelectedItem.ToString() != baseColor || textGroupName.Text != baseName || textGroupDescription.Text != baseDescription || passwordBox.Password != basePassword)
                {
                    buttonSave.IsEnabled = true;
                }
                else
                {
                    buttonSave.IsEnabled = false;
                }
            }
            else
            {
                if (comboBoxColors.SelectedItem.ToString() != baseColor)
                {
                    buttonSave.IsEnabled = true;
                }
                else
                {
                    buttonSave.IsEnabled = false;
                }
            }

        }

        private async void setIsOwner()
        {
            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/get_group_status.php?gid={0}&uid={1}",
                    group.Id,
                    ApplicationData.Current.LocalSettings.Values["Email"]);

            string ResponseString = await httpClient.GetStringAsync(new Uri(url));

            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(ResponseString);

            if (jsonMsg.msg == "owner")
            {
                isOwner = true;
            }
            else
            {
                isOwner = false;
            }
        }

        private async void setColor()
        {
            HttpClient httpClient = new HttpClient();
            string url = string.Format("http://www.friendszone.cba.pl/api/get_group_color.php?gid={0}&uid={1}",
                    group.Id,
                    ApplicationData.Current.LocalSettings.Values["Email"]);

            string ResponseString = await httpClient.GetStringAsync(new Uri(url));
            
            Helpers.JsonMsg jsonMsg = JsonConvert.DeserializeObject<Helpers.JsonMsg>(ResponseString);

            baseColor = ColorParser.parseFloatToColor(float.Parse(jsonMsg.msg));
        }
    }
}
