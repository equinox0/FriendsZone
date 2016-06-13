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
using Windows.Storage;
using FriendsZone.WinPhone.Pages.Groups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace FriendsZone.WinPhone
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    
    public sealed partial class MainPage : Page
    {
        

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (isLogedIn())
            {
                buttonLog.Content = "Wyloguj";
                textBlock.Text = ApplicationData.Current.LocalSettings.Values["name"].ToString();
            }
            else
            {
                buttonMap.IsEnabled = false;
                buttonGroups.IsEnabled = false;
            }
            
            if (e.Parameter.ToString() =="reload")
            {
                this.Frame.BackStack.Remove(this.Frame.BackStack.LastOrDefault());
            }

            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
            /*Button.Click += delegate
            {
                var title = string.Format("{0} clicks!", count++);
                Button.Content = title;
            };*/
        }

        private bool isLogedIn()
        {
            var d = ApplicationData.Current.LocalSettings;            

            if (d.Values.ContainsKey("Email") && d.Values.ContainsKey("Name") && d.Values.ContainsKey("Surname") && d.Values.ContainsKey("Password"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void buttonLog_Click(object sender, RoutedEventArgs e)
        {
            if (!isLogedIn()) Frame.Navigate(typeof(LoginPage));
            else
            {
                ApplicationData.Current.LocalSettings.Values.Clear();
                Frame.Navigate(typeof(MainPage), "reload");
            }
        }

        private void buttonGroups_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GroupMenuPage));
        }
    }
}
