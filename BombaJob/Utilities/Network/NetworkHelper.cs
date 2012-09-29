using System;
using System.Collections.Generic;
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
using System.Text;

namespace BombaJob.Utilities.Network
{
    public class NetworkHelper
    {
        public delegate void EventHandler(Object sender, BombaJobEventArgs e);
        public event EventHandler DownloadComplete;
        public event EventHandler DownloadError;
        public event EventHandler DownloadInBackgroundComplete;

        WebClient webClient;

        public bool InBackground;

        #region Constructor
        public NetworkHelper()
        {
            this.webClient = new WebClient();
            this.webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            this.InBackground = false;
        }
        #endregion

        #region Helpers
        private bool hasConnection()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
        #endregion

        #region GET
        public void downloadURL(string url)
        {
            this.downloadURL(url, false);
        }

        public void downloadURL(string url, bool inBackground)
        {
            this.InBackground = inBackground;

            if (this.hasConnection())
                this.webClient.DownloadStringAsync(new System.Uri(url));
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    DownloadError(this, new BombaJobEventArgs(true, AppResources.error_NoInternet, ""));
                });
            }
        }

        private void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (e.Error != null)
                    DownloadError(this, new BombaJobEventArgs(true, e.Error.Message, ""));
                else if (this.InBackground)
                    DownloadInBackgroundComplete(this, new BombaJobEventArgs(false, "", e.Result));
                else
                    DownloadComplete(this, new BombaJobEventArgs(false, "", e.Result));
            });
        }
        #endregion

        #region POST
        public void uploadURL(string url, Dictionary<string, string> postArray)
        {
            if (this.hasConnection())
            {
                this.webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var uri = new Uri(url, UriKind.Absolute);
                
                StringBuilder postData = new StringBuilder();
                int c = 0;
                foreach (KeyValuePair<string, string> kvp in postArray)
                {
                    postData.AppendFormat((c > 0 ? "&" : "") + "{0}={1}", kvp.Key, HttpUtility.UrlEncode(kvp.Value));
                    c++;
                }

                this.webClient.Headers[HttpRequestHeader.ContentLength] = postData.Length.ToString();
                this.webClient.UploadStringCompleted += new UploadStringCompletedEventHandler(webClient_UploadStringCompleted);
                this.webClient.UploadStringAsync(uri, "POST", postData.ToString());
            }
            else
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    DownloadError(this, new BombaJobEventArgs(true, AppResources.error_NoInternet, ""));
                });
            }
        }

        void webClient_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                if (e.Error != null)
                    DownloadError(this, new BombaJobEventArgs(true, e.Error.Message, ""));
                else
                    DownloadComplete(this, new BombaJobEventArgs(false, "", e.Result));
            });
        }
        #endregion
    }
}
