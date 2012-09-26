using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace BombaJob.Utilities
{
    public class BombaJobBasePage : PhoneApplicationPage
    {
        public delegate void EventHandler(Object sender, BombaJobEventArgs e);
        public event EventHandler SyncComplete;
        public event EventHandler SyncError;

        private Popup popup;
        private SplashScreen splashScreen;

        public BombaJobBasePage()
        {
            SupportedOrientations = SupportedPageOrientation.Portrait | SupportedPageOrientation.Landscape;
        }

        public void DoSync()
        {
            if (this.ApplicationBar != null)
                this.ApplicationBar.IsVisible = false;
            this.ShowSyncPopup();
        }

        private void ShowSyncPopup()
        {
            if (this.splashScreen == null)
                this.splashScreen = new SplashScreen();
            this.splashScreen.SplashError += new SplashScreen.EventHandler(splashScreen_SplashError);
            this.splashScreen.SplashComplete += new SplashScreen.EventHandler(splashScreen_SplashComplete);

            this.popup = new Popup();
            this.popup.Child = this.splashScreen;
            this.popup.IsOpen = true;
            this.splashScreen.startSync();
        }

        void splashScreen_SplashComplete(object sender, BombaJobEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                this.popup.IsOpen = false;
                if (this.ApplicationBar != null)
                    this.ApplicationBar.IsVisible = true;
                try
                {
                    SyncComplete(this, new BombaJobEventArgs(e.IsError, "", ""));
                }
                catch { }
            });
        }

        void splashScreen_SplashError(object sender, BombaJobEventArgs e)
        {
            if (e.IsError)
                MessageBox.Show(e.ErrorMessage);
            this.ApplicationBar.IsVisible = true;
            this.Dispatcher.BeginInvoke(() =>
            {
                SyncError(this, new BombaJobEventArgs(e.IsError, e.ErrorMessage, ""));
            });
        }

        public void BuildApplicationBar()
        {
            this.ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-newest.png", UriKind.Relative));
            appBarButton.Text = AppResources.menu_Newest;
            appBarButton.Click += new System.EventHandler(menuNewest_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-jobs.png", UriKind.Relative));
            appBarButton.Text = AppResources.menu_Jobs;
            appBarButton.Click += new System.EventHandler(menuJobsClick);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-people.png", UriKind.Relative));
            appBarButton.Text = AppResources.menu_People;
            appBarButton.Click += new System.EventHandler(menuPeople_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-search.png", UriKind.Relative));
            appBarButton.Text = AppResources.menu_Search;
            appBarButton.Click += new System.EventHandler(menuSearch_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.menu_Post);
            appBarMenuItem.Click += new System.EventHandler(menuPost_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);

            appBarMenuItem = new ApplicationBarMenuItem(AppResources.menu_Settings);
            appBarMenuItem.Click += new System.EventHandler(menuSettings_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);

            appBarMenuItem = new ApplicationBarMenuItem(AppResources.menu_Sync);
            appBarMenuItem.Click += new System.EventHandler(menuSync_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);

            appBarMenuItem = new ApplicationBarMenuItem(AppResources.menu_About);
            appBarMenuItem.Click += new System.EventHandler(menuAbout_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        void menuNewest_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }

        void menuJobsClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Jobs.xaml", UriKind.Relative));
        }

        void menuPeople_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/People.xaml", UriKind.Relative));
        }

        void menuSearch_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Search.xaml", UriKind.Relative));
        }

        void menuSettings_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Settings.xaml", UriKind.Relative));
        }

        void menuPost_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Post.xaml", UriKind.Relative));
        }

        void menuSync_Click(object sender, EventArgs e)
        {
            this.DoSync();
        }

        void menuAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/About.xaml", UriKind.Relative));
        }
    }
}
