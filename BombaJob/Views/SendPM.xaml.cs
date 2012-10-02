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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using BombaJob.Sync;
using BombaJob.Utilities;

namespace BombaJob.Views
{
    public partial class SendPM : BombaJobBasePage
    {
        private Synchronization syncManager;
        private int offerID;

        public SendPM()
        {
            InitializeComponent();
            this.pageTitle.Text = AppResources.offer_Message;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string oid = "";
            if (NavigationContext.QueryString.TryGetValue("oid", out oid))
                this.offerID = int.Parse(oid);
            else
                NavigationService.GoBack();
        }

        private void send_Click(object sender, EventArgs e)
        {
            if (this.syncManager == null)
                this.syncManager = new Synchronization();
            this.syncManager.SyncError += new Synchronization.EventHandler(syncManager_SyncError);
            this.syncManager.SyncComplete += new Synchronization.EventHandler(syncManager_SyncComplete);
            
            if (!this.txtMessage.Text.Trim().Equals(""))
            {
                Dictionary<string, string> postParams = new Dictionary<string, string>();
                postParams.Add("oid", "" + this.offerID);
                postParams.Add("message", this.txtMessage.Text);
                this.syncManager.DoSendPM(postParams);
            }
            else
                MessageBox.Show(AppResources.offer_Message_Empty);
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        void syncManager_SyncComplete(object sender, BombaJobEventArgs e)
        {
            if (MessageBox.Show(AppResources.offer_ThankYou) == MessageBoxResult.OK)
                NavigationService.GoBack();
        }

        void syncManager_SyncError(object sender, BombaJobEventArgs e)
        {
            if (e.IsError)
                MessageBox.Show(e.ErrorMessage);
        }
    }
}