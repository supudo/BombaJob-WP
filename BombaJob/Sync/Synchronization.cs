using System;
using System.Net;
using System.Windows;
using System.Threading;
using BombaJob.Workers.Network;
using BombaJob.Workers;

namespace BombaJob.Sync
{
    public class Synchronization
    {
        public delegate void EventHandler(Object sender, BJEventArgs e);
        public event EventHandler SyncComplete;
        public event EventHandler SyncError;

        NetworkHelper _networkHelper;

        private ServiceOp currentOp;
        enum ServiceOp
        {
            ServiceOpTexts,
            ServiceOpCategories,
            ServiceOpJobs
        }

        public Synchronization()
        {
            this._networkHelper = new NetworkHelper();
            this._networkHelper.DownloadComplete += new NetworkHelper.EventHandler(_networkHelper_DownloadComplete);
            this._networkHelper.DownloadError += new NetworkHelper.EventHandler(_networkHelper_DownloadError);
        }

        public void StartSync()
        {
            this.syncTexts();
        }

        private void SynchronizationComplete()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncComplete(this, new BJEventArgs(false, "", ""));
            });
        }

        #region Dispatcher
        private void dispatchDownload(string xmlContent)
        {
            switch (this.currentOp)
            {
                case ServiceOp.ServiceOpTexts:
                    this.doTexts(xmlContent);
                    break;
                case ServiceOp.ServiceOpCategories:
                    this.doCategories(xmlContent);
                    break;
                case ServiceOp.ServiceOpJobs:
                    this.doJobOffers(xmlContent);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Handle URLs
        private void syncTexts()
        {
            this.currentOp = ServiceOp.ServiceOpTexts;
            this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=getTextContent");
        }

        private void syncCategories()
        {
            this.currentOp = ServiceOp.ServiceOpCategories;
            this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=getCategories");
        }
        #endregion

        #region Network Events
        void _networkHelper_DownloadComplete(object sender, BJEventArgs e)
        {
            if (!e.IsError)
                this.dispatchDownload(e.XmlContent);
        }

        void _networkHelper_DownloadError(object sender, BJEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncError(this, new BJEventArgs(true, e.ErrorMessage, ""));
            });
        }
        #endregion

        #region Initial Synchronization
        private void doTexts(string xmlContent)
        {
            this.syncCategories();
        }

        private void doCategories(string xmlContent)
        {
            this.SynchronizationComplete();
        }
        #endregion

        #region Parsers
        private void doJobOffers(string xmlContent)
        {
        }
        #endregion
    }
}
