using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BombaJob.Utilities;

namespace BombaJob.Views
{
    public partial class Settings : BombaJobBasePage
    {
        public Settings()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.menu_Settings;
            this.Loaded += new RoutedEventHandler(Settings_Loaded);
        }

        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            this.chkPrivateData.Content = AppResources.conf_PrivateData;
            this.chkPrivateData.IsChecked = AppSettings.ConfPrivateData;

            this.chkInitSync.Content = AppResources.conf_InitSync;
            this.chkInitSync.IsChecked = AppSettings.ConfInitSync;

            this.chkOnlineSearch.Content = AppResources.conf_OnlineSearch;
            this.chkOnlineSearch.IsChecked = AppSettings.ConfOnlineSearch;

            this.chkInAppEmail.Content = AppResources.conf_InAppEmail;
            this.chkInAppEmail.IsChecked = AppSettings.ConfInAppEmail;

            this.chkShowCategories.Content = AppResources.conf_ShowCategories;
            this.chkShowCategories.IsChecked = AppSettings.ConfShowCategories;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            AppSettings.ConfPrivateData = (bool)this.chkPrivateData.IsChecked;
            AppSettings.ConfInitSync = (bool)this.chkInitSync.IsChecked;
            AppSettings.ConfOnlineSearch = (bool)this.chkOnlineSearch.IsChecked;
            AppSettings.ConfInAppEmail = (bool)this.chkInAppEmail.IsChecked;
            AppSettings.ConfShowCategories = (bool)this.chkShowCategories.IsChecked;

            if (!AppSettings.ConfPrivateData)
                AppSettings.ConfPDEmail = "";
            
            NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }
    }
}