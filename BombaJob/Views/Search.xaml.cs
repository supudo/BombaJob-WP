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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BombaJob.Sync;
using BombaJob.Utilities;

namespace BombaJob.Utilities.Views
{
    public partial class Search : BombaJobBasePage
    {
        private Synchronization syncManager;

        public Search()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.menu_Search;
            this.Loaded += new RoutedEventHandler(Search_Loaded);
        }

        void Search_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> fl = new List<string>();
            fl.Add(AppResources.freelance_all);
            fl.Add(AppResources.freelance_only);
            fl.Add(AppResources.freelance_no);
            this.chkFreelanceYn.ItemsSource = fl;
        }

        private void searchOK_Click(object sender, EventArgs e)
        {
            if (AppSettings.ConfOnlineSearch)
            {
                if (this.syncManager == null)
                    this.syncManager = new Synchronization();
                this.syncManager.SyncError += new Synchronization.EventHandler(syncManager_SyncError);
                this.syncManager.SyncComplete += new Synchronization.EventHandler(syncManager_SyncComplete);
                this.syncManager.DoSearch(this.txtKeyword.Text, this.chkFreelanceYn.SelectedIndex);
            }
            else
                NavigationService.Navigate(new Uri("/Views/SearchResults.xaml?k=" + this.txtKeyword.Text + "&f=" + this.chkFreelanceYn.SelectedIndex, UriKind.Relative));
        }

        private void searchCancel_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/Newest.xaml", UriKind.Relative));
        }

        void syncManager_SyncComplete(object sender, BombaJobEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                NavigationService.Navigate(new Uri("/Views/SearchResults.xaml?k=" + this.txtKeyword.Text + "&f=" + this.chkFreelanceYn.SelectedIndex, UriKind.Relative));
            });
        }

        void syncManager_SyncError(object sender, BombaJobEventArgs e)
        {
            if (e.IsError)
                MessageBox.Show(e.ErrorMessage);
        }
    }
}