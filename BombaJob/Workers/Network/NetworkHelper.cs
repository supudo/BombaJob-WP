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
using Microsoft.Phone.Net.NetworkInformation;

namespace BombaJob.Workers.Network
{
    public class NetworkHelper
    {
        public delegate void EventHandler(Object sender, BJEventArgs e);
        public event EventHandler DownloadComplete;
        public event EventHandler DownloadError;

        WebClient webClient;

        public NetworkHelper()
        {
            this.webClient = new WebClient();
            this.webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
        }

        private bool hasConnection()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }

        public void downloadURL(string url)
        {
            if (this.hasConnection())
                webClient.DownloadStringAsync(new System.Uri(url));
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    DownloadError(this, new BJEventArgs(true, AppResources.error_NoInternet, ""));
                });
            }
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (e.Error != null)
                    DownloadError(this, new BJEventArgs(true, e.Error.Message, ""));
                else
                    DownloadComplete(this, new BJEventArgs(false, "", e.Result));
            });
        }
    }
}
