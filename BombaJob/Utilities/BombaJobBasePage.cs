using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
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
        public void BuildApplicationBar()
        {
            this.ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-newest.png", UriKind.Relative));
            appBarButton.Text = AppResources.menu_Newest;
            appBarButton.Click += new EventHandler(menuNewest_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-jobs.png", UriKind.Relative));
            appBarButton.Text = AppResources.menu_Jobs;
            appBarButton.Click += new EventHandler(menuJobsClick);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-people.png", UriKind.Relative));
            appBarButton.Text = AppResources.menu_People;
            appBarButton.Click += new EventHandler(menuPeople_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            appBarButton = new ApplicationBarIconButton(new Uri("images/menu/tb-search.png", UriKind.Relative));
            appBarButton.Text = AppResources.menu_Search;
            appBarButton.Click += new EventHandler(menuSearch_Click);
            this.ApplicationBar.Buttons.Add(appBarButton);

            ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.menu_Post);
            appBarMenuItem.Click += new EventHandler(menuPost_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);

            appBarMenuItem = new ApplicationBarMenuItem(AppResources.menu_Settings);
            appBarMenuItem.Click += new EventHandler(menuSettings_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);

            appBarMenuItem = new ApplicationBarMenuItem(AppResources.menu_Sync);
            appBarMenuItem.Click += new EventHandler(menuSync_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);

            appBarMenuItem = new ApplicationBarMenuItem(AppResources.menu_About);
            appBarMenuItem.Click += new EventHandler(menuAbout_Click);
            this.ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        void menuNewest_Click(object sender, EventArgs e)
        {
        }

        void menuJobsClick(object sender, EventArgs e)
        {
        }

        void menuPeople_Click(object sender, EventArgs e)
        {
        }

        void menuSearch_Click(object sender, EventArgs e)
        {
        }

        void menuSettings_Click(object sender, EventArgs e)
        {
        }

        void menuPost_Click(object sender, EventArgs e)
        {
        }

        void menuSync_Click(object sender, EventArgs e)
        {
        }

        void menuAbout_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/About.xaml", UriKind.Relative));
        }
    }
}
