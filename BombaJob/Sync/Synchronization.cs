using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using BombaJob.Database;
using BombaJob.Database.Tables;
using BombaJob.Utilities.Network;
using BombaJob.Utilities;

namespace BombaJob.Sync
{
    public class Synchronization
    {
        public delegate void EventHandler(Object sender, BombaJobEventArgs e);
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
                SyncComplete(this, new BombaJobEventArgs(false, "", ""));
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
        void _networkHelper_DownloadComplete(object sender, BombaJobEventArgs e)
        {
            if (!e.IsError)
                this.dispatchDownload(e.XmlContent);
        }

        void _networkHelper_DownloadError(object sender, BombaJobEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncError(this, new BombaJobEventArgs(true, e.ErrorMessage, ""));
            });
        }
        #endregion

        #region Initial Synchronization
        private void doTexts(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var texts = from txt in doc.Descendants("tctxt")
                        select new Texts
                        {
                            TextId = int.Parse(txt.Attribute("id").Value),
                            Title = txt.Element("tctitle").Value,
                            Content = txt.Element("tccontent").Value,
                        };
            using (BombaJobDataContext db = new BombaJobDataContext(AppSettings.DBConnectionString))
            {
                foreach (Texts t in texts)
                    App.DbViewModel.AddText(t);
            }
            this.syncCategories();
        }

        private void doCategories(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var cats = from cat in doc.Descendants("cat")
                        select new Categories
                        {
                            CategoryId = int.Parse(cat.Attribute("id").Value),
                            OffersCount = int.Parse(cat.Attribute("cnt").Value),
                            Title = cat.Element("cttl").Value,
                        };
            using (BombaJobDataContext db = new BombaJobDataContext(AppSettings.DBConnectionString))
            {
                foreach (Categories t in cats)
                    App.DbViewModel.AddCategory(t);
            }
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
