using System;
using System.Net;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Xml.Linq;
using BombaJob.Database.Context;
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
            ServiceOpNewestOffers,
            ServiceOpSearch
        }

        #region Constructor
        public Synchronization()
        {
            this._networkHelper = new NetworkHelper();
            this._networkHelper.DownloadComplete += new NetworkHelper.EventHandler(_networkHelper_DownloadComplete);
            this._networkHelper.DownloadError += new NetworkHelper.EventHandler(_networkHelper_DownloadError);
        }
        #endregion

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
                case ServiceOp.ServiceOpNewestOffers:
                    this.doNewestOffers(xmlContent);
                    break;
                case ServiceOp.ServiceOpSearch:
                    this.doJobOffers(xmlContent);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Public
        public void DoSearch(string keyword, int freelance)
        {
            this.currentOp = ServiceOp.ServiceOpSearch;
            this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=searchOffers?keyword=" + keyword + "&freelance=" + freelance);
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

        private void syncNewestOffers()
        {
            this.currentOp = ServiceOp.ServiceOpNewestOffers;
            this._networkHelper.downloadURL(AppSettings.ServicesURL + "?action=getNewJobs");
        }
        #endregion

        #region Events
        private void SynchronizationComplete()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncComplete(this, new BombaJobEventArgs(false, "", ""));
            });
        }

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
        public void StartSync()
        {
            this.syncTexts();
        }

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
            this.syncNewestOffers();
        }

        private void doNewestOffers(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            var jobs = from job in doc.Descendants("job")
                       select new JobOffers
                       {
                           OfferId = int.Parse(job.Attribute("id").Value),
                           CategoryId = int.Parse(job.Attribute("cid").Value),
                           HumanYn = int.Parse(job.Attribute("hm").Value) == 1,
                           FreelanceYn = int.Parse(job.Attribute("fyn").Value) == 1,
                           Title = job.Element("jottl").Value,
                           Email = job.Element("joem").Value,
                           CategoryTitle = job.Element("jocat").Value,
                           Positivism = job.Element("jopos").Value,
                           Negativism = job.Element("joneg").Value,
                           PublishDate = DateTime.ParseExact(job.Element("jodt").Value, AppSettings.DateTimeFormat, null),
                           Icon = ((int.Parse(job.Attribute("hm").Value) == 1) ? "iconperson" : "iconcompany"),
                       };
            using (BombaJobDataContext db = new BombaJobDataContext(AppSettings.DBConnectionString))
            {
                foreach (JobOffers t in jobs)
                    App.DbViewModel.AddJobOffer(t);
            }
            this.SynchronizationComplete();
        }
        #endregion

        #region Parsers
        private void doJobOffers(string xmlContent)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlContent);
                var jobs = from job in doc.Descendants("job")
                           select new JobOffers
                           {
                               OfferId = int.Parse(job.Attribute("id").Value),
                               CategoryId = int.Parse(job.Attribute("cid").Value),
                               HumanYn = int.Parse(job.Attribute("hm").Value) == 1,
                               FreelanceYn = int.Parse(job.Attribute("fyn").Value) == 1,
                               Title = job.Element("jottl").Value,
                               Email = job.Element("joem").Value,
                               CategoryTitle = job.Element("jocat").Value,
                               Positivism = job.Element("jopos").Value,
                               Negativism = job.Element("joneg").Value,
                               PublishDate = DateTime.ParseExact(job.Element("jodt").Value, AppSettings.DateTimeFormat, null),
                               Icon = ((int.Parse(job.Attribute("hm").Value) == 1) ? "iconperson" : "iconcompany"),
                           };
                using (BombaJobDataContext db = new BombaJobDataContext(AppSettings.DBConnectionString))
                {
                    foreach (JobOffers t in jobs)
                        App.DbViewModel.AddJobOffer(t);
                }
            }
            catch (Exception e)
            {
                AppSettings.LogThis("Synchronization - doJobOffers - " + e.ToString());
            }
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                SyncComplete(this, new BombaJobEventArgs(false, "", ""));
            });
        }
        #endregion
    }
}
